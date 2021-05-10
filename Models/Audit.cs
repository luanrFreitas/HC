using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleCastle.Models
{
    public class Audit
    {

        public Audit()
        {

        }
        public Audit(string user, string action, string table, string item)
        {
            User = user;
            Action = action;
            Table = table;
            Item = item;
            MakedAt = DateTime.UtcNow.AddHours(-3).ToString();
        }
        public int Id { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string Table { get; set; }
        public string Item { get; set; }
        public string MakedAt { get; set; }
    }
}
