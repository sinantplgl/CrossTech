using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLayer
{
    public class FightLogService : IFightLogService
    {
        CrossTechFighterContext _context;
        public FightLogService(CrossTechFighterContext context)
        {
            _context = context;
        }
        public FightLog CreateFightLog(FightLog fightLog)
        {
            _context.FightLogs.Add(fightLog);
            _context.SaveChanges();

            return fightLog;
        }

        public IEnumerable<FightLog> GetAllLogs()
        {
            return _context.FightLogs.ToList();
        }

        public FightLog GetFightLog(Guid id)
        {
            return _context.FightLogs.Find(id);
        }

        public IEnumerable<FightLog> GetLogsByFightId(Guid fightId)
        {
            return _context.FightLogs
                .Where(log => log.FightId == fightId)
                .ToList();
        }
    }
}
