using Discord;
using Discord.WebSocket;
using DiscordSelfUI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace DiscordSelfUI.Controllers
{
    public class RaidController(IMainClient client) : Controller
    {
        [Route("Raid/{id}")]
        public IActionResult Index(ulong id)
        {
            if (client.MainClient == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var guild = client.MainClient.GetGuild(id);

            if (guild == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var discordUserId = HttpContext.Session.GetString("DiscordUserId");

            var serverModel = new RaidModel
            {
                Id = guild.Id,
                Name = guild.Name,
                Description = guild.Description,
                IconUrl = guild.IconUrl,
                TextChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Text).ToList(),
                VoiceChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Voice).ToList()
            };

            ViewBag.DiscordUserId = discordUserId;

            return View(serverModel);
        }

        [HttpPost]
        [Route("Raid/{id}")]
        public async Task<IActionResult> Index(RaidModel model)
        {
            if (model.File != null && model.File.Length > 0)
            {
                using var stream = new MemoryStream();
                await model.File.CopyToAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream);
                model.Bots = new List<DiscordSocketClient>();

                string? line;
                var tokens = new List<string>();
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (Regex.Match(line.Trim(), "^(mfa\\.[a-z0-9_-]{20,}|[a-zA-Z0-9_-]{23,28}\\.[a-zA-Z0-9_-]{6,7}\\.[a-zA-Z0-9_-]{27,})$").Success)
                    {
                        tokens.Add(line.Trim().Split("\t")[0]);
                    }
                }

                var bots = await LoadMultipleBots(tokens);
                foreach (var bot in bots)
                {
                    if (bot != null)
                    {
                        if (!model.Bots.Contains(bot))
                            model.Bots.Add(bot);
                    }
                }

                var guild = client.MainClient.GetGuild(model.Id);

                if (guild == null)
                {
                    return RedirectToAction(nameof(Index), "Raid");
                }

                model.Id = guild.Id;
                model.Name = guild.Name;
                model.Description = guild.Description;
                model.IconUrl = guild.IconUrl;
                model.TextChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Text).ToList();
                model.VoiceChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Voice).ToList();

                return View(nameof(Index), model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<DiscordSocketClient?[]> LoadMultipleBots(IEnumerable<string> tokens)
        {
            var tasks = tokens.Select(token => LoadBot(token)).ToList();
            return await Task.WhenAll(tasks);
        }

        public async Task<DiscordSocketClient?> LoadBot(string token)
        {
            try
            {
                var bot = new DiscordSocketClient();
                if (bot.ConnectionState != ConnectionState.Connected)
                {
                    await bot.LoginAsync(TokenType.User, token);
                    await bot.StartAsync();
                }

                var timeoutTask = Task.Delay(3000);
                while (bot.ConnectionState != ConnectionState.Connected && !timeoutTask.IsCompleted)
                {
                    await Task.Delay(100);
                }

                return bot.ConnectionState == ConnectionState.Connected ? bot : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
