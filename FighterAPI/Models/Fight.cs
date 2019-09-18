using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FighterAPI.Models
{
    public class Fight
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid BotId { get; set; }

        public Player Player { get; set; }
        public Player Bot { get; set; }
        public virtual IEnumerable<FightLog> FightLogs { get; set; }
    }
}
