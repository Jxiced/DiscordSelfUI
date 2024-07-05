using Discord.WebSocket;

namespace DiscordSelfUI.Data
{
    public class UserModel
    {
        public ulong Id { get; set; }

        public string Username { get; set; }

        public string AvatarUrl { get; set; }
    }
}
