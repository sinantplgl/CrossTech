using DataAccessLayer.Interfaces;
using FighterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FighterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightLogsController : Controller
    {
        private IFightLogService _fightLogService;

        public FightLogsController(IFightLogService fightLogService)
        {
            _fightLogService = fightLogService;
        }

        [HttpGet]
        public IEnumerable<FightLog> Get()
        {
            return _fightLogService.GetAllLogs()
                .Select(log => new FightLog
                {
                    Id = log.Id,
                    FightId = log.FightId,
                    Turn = log.Turn,
                    PlayerHitPoint = log.PlayerHitPoint,
                    BotHitPoint = log.BotHitPoint,
                    LogEntry = log.LogEntry
                });
        }

        [HttpGet("{id}")]
        public ActionResult<FightLog> Get(Guid id)
        {
            var fightLog = _fightLogService.GetFightLog(id);
            if (fightLog == null)
                return NotFound();
            return new FightLog
            {
                Id = fightLog.Id,
                FightId = fightLog.FightId,
                Turn = fightLog.Turn,
                PlayerHitPoint = fightLog.PlayerHitPoint,
                BotHitPoint = fightLog.BotHitPoint,
                LogEntry = fightLog.LogEntry
            };
        }

        [HttpGet("fight/{id}")]
        public IEnumerable<FightLog> GetByFightId(Guid id)
        {
            return _fightLogService.GetLogsByFightId(id)
                .Select(log => new FightLog
                {
                    Id = log.Id,
                    Turn = log.Turn,
                    PlayerHitPoint = log.PlayerHitPoint,
                    BotHitPoint = log.BotHitPoint,
                    LogEntry = log.LogEntry
                })
                .OrderByDescending(log => log.Turn);
        }
    }
}
