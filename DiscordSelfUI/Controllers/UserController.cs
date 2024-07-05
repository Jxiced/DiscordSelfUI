using DiscordSelfUI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DiscordSelfUI.Controllers
{
    public class UserController(IMainClient client) : Controller
    {
        [Route("User/{id}")]
        public async Task<IActionResult> Index(ulong id)
        {
            var user = await client.MainClient.GetUserAsync(id);
            var avatarUrl = user.GetAvatarUrl();

            var userModel = new UserModel
            {
                Id = user.Id,
                Username = user.Username,
                AvatarUrl = avatarUrl ?? user.GetDefaultAvatarUrl()
            };

            return View(userModel);
        }
    }
}
