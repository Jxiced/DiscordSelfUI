using Discord;
using Discord.WebSocket;

namespace DiscordSelfUI.Data
{
    public class CurrentUserModel : UserModel
    {
        public string Email { get; set; }

        public List<SocketGuild> Guilds { get; set; }

        public IEnumerable<IDMChannel> Friends { get; set; }
    }
}
