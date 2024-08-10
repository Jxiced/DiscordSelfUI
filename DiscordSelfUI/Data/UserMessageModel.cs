using Discord;

namespace DiscordSelfUI.Data
{
    public class UserMessageModel() : UserModel
    {
        public int MaxMessageLength { get; set; }

        public string? Message { get; set; }

        public bool TTS { get; set; }
    }
}
