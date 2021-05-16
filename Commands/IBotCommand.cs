using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Commands
{
    public interface IBotCommand
    {
        string Command { get; }
        string Description { get; }
        bool InternalCommand { get; }

        Task Execute(IChatService chatService, long chatId, int userId, int messageId, string? commandText);
    }
}
