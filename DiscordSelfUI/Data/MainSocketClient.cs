using Discord;
using Discord.WebSocket;

namespace DiscordSelfUI.Data
{
    public class MainSocketClient : IMainClientSingleton
    {
        public MainSocketClient() : this(new DiscordSocketClient())
        {
            
        }

        DiscordSocketClient _client;

        public MainSocketClient(DiscordSocketClient client)
        {
            _client = client;
        }

        public DiscordSocketClient MainClient => _client;
    }

    public interface IMainClient
    {
        DiscordSocketClient MainClient { get; }
    }

    public interface IMainClientSingleton : IMainClient
    {
        
    }
}
