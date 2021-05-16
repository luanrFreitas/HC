using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Commands
{
    public class TestCommand : IBotCommand
    {
        public string Command => "test";

        public string Description => "This is a simple command that can be used to test if the bot is online";

        public bool InternalCommand => false;

        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
            await chatService.SendMessage(chatId, "testado");
        }
    }
}
