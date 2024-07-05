using Discord.WebSocket;

namespace DiscordSelfUI.Data
{
    public class ServerModel
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string? IconUrl { get; set; }

        public string? Description { get; set; }

        public List<SocketGuildChannel> TextChannels { get; set; }

        public List<SocketGuildChannel> VoiceChannels { get; set; }
    }
}
