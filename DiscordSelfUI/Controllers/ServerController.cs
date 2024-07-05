using Discord;
using DiscordSelfUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DiscordSelfUI.Controllers
{
    public class ServerController(IMainClient client) : Controller
    {
        [Route("Server/{id}")]
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

            var serverModel = new ServerModel
            {
                Id = guild.Id,
                Name = guild.Name,
                Description = guild.Description,
                IconUrl = guild.IconUrl,
                TextChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Text).ToList(),
                VoiceChannels = guild.Channels.Where(x => x.GetChannelType() == ChannelType.Voice).ToList()
            };

            return View(serverModel);
        }

        public async Task<IActionResult> LeaveServer(ulong id)
        {
            var guild = client.MainClient.GetGuild(id);
            await guild.LeaveAsync();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
