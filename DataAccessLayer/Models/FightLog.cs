using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Models
{
    [Table("FightLogs")]
    public class FightLog
    {
        public Guid Id { get; set; }
        public Guid FightId { get; set; }
        public int Turn { get; set; }
        public int PlayerHitPoint { get; set; }
        public int BotHitPoint { get; set; }
        public string LogEntry { get; set; }

        public virtual Fight Fight { get; set; }
    }
}
