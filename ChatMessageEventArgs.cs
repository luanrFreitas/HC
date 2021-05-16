using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle
{
    public class ChatMessageEventArgs
    {
        public string? Command { get; set; }
        public string? Text { get; set; }
        public int UserId { get; set; }
        public long ChatId { get; set; }
        public int MessageId { get; set; }
    }
}
