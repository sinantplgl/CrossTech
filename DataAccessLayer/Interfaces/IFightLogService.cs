using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IFightLogService
    {
        IEnumerable<FightLog> GetAllLogs();
        IEnumerable<FightLog> GetLogsByFightId(Guid fightId);
        FightLog GetFightLog(Guid id);
        FightLog CreateFightLog(FightLog fightLog);
    }
}
