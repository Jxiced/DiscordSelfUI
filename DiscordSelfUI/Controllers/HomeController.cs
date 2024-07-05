using Discord;
using DiscordSelfUI.Data;
using DiscordSelfUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DiscordSelfUI.Controllers
{
    public class HomeController(IConfiguration configuration, IMainClient client) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var token = configuration["UserToken"];

            if (client.MainClient.ConnectionState != ConnectionState.Connected)
            {
                await client.MainClient.LoginAsync(TokenType.User, token);
                await client.MainClient.StartAsync();
            }

            while (client.MainClient.ConnectionState != ConnectionState.Connected)
            {
                //dumb, but discord.net requires this
            }

            HttpContext.Session.SetString("DiscordUserId", client.MainClient.CurrentUser.Id.ToString());

            var avatarUrl = client.MainClient.CurrentUser.GetAvatarUrl();
            var userModel = new CurrentUserModel
            {
                Id = client.MainClient.CurrentUser.Id,
                Username = client.MainClient.CurrentUser.Username,
                Email = client.MainClient.CurrentUser.Email,
                AvatarUrl = avatarUrl ?? client.MainClient.CurrentUser.GetDefaultAvatarUrl(),
                Guilds = client.MainClient.Guilds.ToList(),
                Friends = await client.MainClient.GetDMChannelsAsync()
            };

            return View(userModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
