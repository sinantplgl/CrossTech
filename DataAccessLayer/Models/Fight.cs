using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Models
{
    [Table("Fights")]
    public class Fight
    {
        public Fight()
        {
            FightLogs = new HashSet<FightLog>();
        }

        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid BotId { get; set; }

        public virtual Player Player { get; set; }
        public virtual Player Bot { get; set; }
        public virtual IEnumerable<FightLog> FightLogs { get; set; }
    }
}
