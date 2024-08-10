using Discord;
using Discord.WebSocket;
using DiscordSelfUI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DiscordSelfUI.Controllers
{
    public class UserController(IMainClient client) : Controller
    {
        [Route("User/{id}")]
        public async Task<IActionResult> Index(ulong id)
        {
            var socketUser = client.MainClient.GetUser(id);

            if (socketUser == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var user = await client.MainClient.GetUserAsync(id);

            var avatarUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();

            var mutualGuilds = socketUser?.MutualGuilds;

            var userModel = new UserModel
            {
                Id = user.Id,
                Username = user.Username,
                AvatarUrl = avatarUrl,
                MutualGuilds = mutualGuilds != null ? client.MainClient.Guilds.Intersect(mutualGuilds) : null
            };

            return View(userModel);
        }

        [HttpGet]
        [Route("/User/{id}/Message")]
        public async Task<IActionResult> Message(ulong id)
        {
            var socketUser = client.MainClient.GetUser(id);

            if (socketUser == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var user = await client.MainClient.GetUserAsync(id);

            var avatarUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();

            var mutualGuilds = socketUser?.MutualGuilds;

            var nitroType = client.MainClient.CurrentUser.PremiumType;

            var userModel = new UserMessageModel()
            {
                Id = user.Id,
                Username = user.Username,
                AvatarUrl = avatarUrl,
                MutualGuilds = mutualGuilds != null ? client.MainClient.Guilds.Intersect(mutualGuilds) : null,
                MaxMessageLength = (nitroType == PremiumType.None && nitroType == PremiumType.NitroBasic) ? 2000 : 4000
            };

            return View(userModel);
        }

        [HttpPost]
        [Route("/User/{id}/Message")]
        public async Task<IActionResult> Message(UserMessageModel model)
        {
            if (string.IsNullOrEmpty(model.Message) || string.IsNullOrWhiteSpace(model.Message))
            {
                return Json(new { Error = true });
            }

            var socketUser = await client.MainClient.GetUserAsync(model.Id);

            if (socketUser == null)
            {
                return Json(new { Error = true });
            }

            var nitroType = client.MainClient.CurrentUser.PremiumType;
            var messageLength = 2000;

            if (nitroType != PremiumType.NitroBasic && nitroType != PremiumType.None && messageLength == 2000)
            {
                messageLength += messageLength;
            }

            if (model.Message.Length < messageLength)
            {
                await socketUser.SendMessageAsync(model.Message, model.TTS);
            }
            else
            {
                var stream = new MemoryStream();
                var textBytes = Encoding.ASCII.GetBytes(model.Message);
                await stream.WriteAsync(textBytes);

                await socketUser.SendFileAsync(stream, "message.txt", null, model.TTS);
            }

            return RedirectToAction(nameof(Message), "User");
        }
    }
}
