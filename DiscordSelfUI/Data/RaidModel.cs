using Discord.WebSocket;

namespace DiscordSelfUI.Data
{
    public class RaidModel : ServerModel
    {
        public List<DiscordSocketClient>? Bots { get; set; }

        public IFormFile File { get; set; }
    }
}
