using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Commands
{
    public class AjudaCommand : IBotCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public string Command => "ajuda";

        public string Description => "Dá informações sobre as funções do bot";

        public bool InternalCommand => false;

        public AjudaCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText)
        {
            await chatService.SendMessage(chatId, "TODO: Create a todo command");
        }
    }
}
