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
    public class FightsController : Controller
    {
        private IFightService _fightService;
        private IPlayerService _playerService;
        private IFightLogService _fightLogService;

        public FightsController(
            IFightService fightService, 
            IPlayerService playerService, 
            IFightLogService fightLogService)
        {
            _fightService = fightService;
            _playerService = playerService;
            _fightLogService = fightLogService;
        }

        [HttpGet]
        public IEnumerable<Fight> Get()
        {
            return _fightService.GetAllFights()
                .Select(result => new Fight
                {
                    Id = result.Id,
                    PlayerId = result.PlayerId,
                    BotId = result.BotId,
                    FightLogs = result.FightLogs.Select(s => new FightLog
                    {
                        Id = s.Id,
                        Turn = s.Turn,
                        PlayerHitPoint = s.PlayerHitPoint,
                        BotHitPoint = s.BotHitPoint,
                        LogEntry = s.LogEntry
                    }).OrderByDescending(log => log.Turn)
                });
        }

        [HttpGet("{id}")]
        public ActionResult<Fight> Get(Guid id)
        {
            var result = _fightService.GetFightById(id);
            if (result == null)
                return NotFound();
            return new Fight
            {
                Id = result.Id,
                PlayerId = result.PlayerId,
                BotId = result.BotId,
                FightLogs = result.FightLogs.Select(s => new FightLog
                {
                    Id = s.Id,
                    Turn = s.Turn,
                    PlayerHitPoint = s.PlayerHitPoint,
                    BotHitPoint = s.BotHitPoint,
                    LogEntry = s.LogEntry
                }).OrderByDescending(log => log.Turn)
            };
        }

        [HttpGet("start/{id}")]
        public ActionResult<Fight> StartFight(Guid id)
        {
            //Create Bot with default vaules for new fight
            var botId = Guid.Parse("4cb89d9f-3fd9-e911-a5ff-80fa5b0fc197");
            var bot = _playerService.GetPlayer(botId);
            
            //Get player
            var player = _playerService.GetPlayer(id);

            //Create new fight between player and bot
            var fight = _fightService.CreateFight(
                new DataAccessLayer.Models.Fight
                {
                    PlayerId = id,
                    BotId = bot.Id
                });

            //Add initial fight log to database
            var log = _fightLogService.CreateFightLog(
                new DataAccessLayer.Models.FightLog
                {
                    FightId = fight.Id,
                    PlayerHitPoint = player.HitPoint,
                    BotHitPoint = bot.HitPoint,
                    Turn = 0,
                    LogEntry = String.Format("Fight started between Player: {0} and Bot: {1}", player.Id, bot.Id)
                });

            return Ok(new Fight
            {
                Id = fight.Id,
                PlayerId = fight.PlayerId,
                BotId = fight.BotId,
                FightLogs = _fightLogService
                    .GetLogsByFightId(fight.Id)
                    .ToList()
                    .Select(l => new FightLog
                    {
                        Id = l.Id,
                        PlayerHitPoint = l.PlayerHitPoint,
                        BotHitPoint = l.BotHitPoint,
                        Turn = l.Turn,
                        LogEntry = l.LogEntry
                    })
            });
        }

        [HttpGet("{id}/attack")]
        public ActionResult<Fight> RegularAttack(Guid id)
        {
            //Get fight information
            var fight = _fightService.GetFightById(id);
            var player = _playerService.GetPlayer(fight.PlayerId);
            var bot = _playerService.GetPlayer(fight.BotId);
            var lastLog = _fightLogService.GetLogsByFightId(fight.Id).LastOrDefault();

            //Players makes a regular attack
            if (RollAttack(bot.ArmorClass)) //Attack hits
            {
                int currentHP = lastLog.BotHitPoint;
                if(currentHP > player.Damage) //If its not the final blow
                {
                   lastLog = _fightLogService.CreateFightLog(
                        new DataAccessLayer.Models.FightLog
                        {
                            FightId = fight.Id,
                            PlayerHitPoint = lastLog.PlayerHitPoint,
                            BotHitPoint = lastLog.BotHitPoint - player.Damage,
                            Turn = lastLog.Turn + 1,
                            LogEntry = String.Format("Player {0}: Dealt {1} damage to the Bot {2}.", player.Id, player.Damage, bot.Id)
                        });
                    //Bot makes an attack
                    BotAttack(fight, player, bot, lastLog);
                }
                else
                {

                    lastLog = _fightLogService.CreateFightLog(
                        new DataAccessLayer.Models.FightLog
                        {
                            FightId = fight.Id,
                            PlayerHitPoint = lastLog.PlayerHitPoint,
                            BotHitPoint = 0,
                            Turn = lastLog.Turn + 1,
                            LogEntry = String.Format("Player {0}: Dealt {1} damage to the Bot {2}. Player won!", player.Id, lastLog.BotHitPoint, bot.Id)
                        });
                }
            }
            else
            {
                lastLog =_fightLogService.CreateFightLog(
                    new DataAccessLayer.Models.FightLog
                    {
                        FightId = fight.Id,
                        PlayerHitPoint = lastLog.PlayerHitPoint,
                        BotHitPoint = lastLog.BotHitPoint,
                        Turn = lastLog.Turn + 1,
                        LogEntry = String.Format("Player {0}: Couldn't exceed the armor class.", player.Id)
                    });
                //Bot makes an attack
                BotAttack(fight, player, bot, lastLog);
            }

            return Ok(new Fight
            {
                Id = fight.Id,
                PlayerId = fight.PlayerId,
                BotId = fight.BotId,
                FightLogs = _fightLogService
                    .GetLogsByFightId(fight.Id)
                    .ToList()
                    .Select(l => new FightLog
                    {
                        Id = l.Id,
                        PlayerHitPoint = l.PlayerHitPoint,
                        BotHitPoint = l.BotHitPoint,
                        Turn = l.Turn,
                        LogEntry = l.LogEntry
                    }).OrderByDescending(log => log.Turn)
            });
        }

        [HttpGet("{id}/ability/{abilityId}")]
        public ActionResult<Fight> SpecialAttack(Guid id, Guid abilityId)
        {
            //Get fight information
            var fight = _fightService.GetFightById(id);
            var player = _playerService.GetPlayer(fight.PlayerId);
            var bot = _playerService.GetPlayer(fight.BotId);
            var lastLog = _fightLogService.GetLogsByFightId(fight.Id).LastOrDefault();
            //Player uses an ability

            //Bot makes an attack
            BotAttack(fight, player, bot, lastLog);
            return Ok();
        }

        private void BotAttack(
            DataAccessLayer.Models.Fight fight, DataAccessLayer.Models.Player player,
            DataAccessLayer.Models.Player bot, DataAccessLayer.Models.FightLog lastLog)
        {
            //Bot rolls for an attack
            if(RollAttack(player.ArmorClass))
            {
                int currentHP = lastLog.PlayerHitPoint;
                //TODO: Bot can make a special attack
                if (currentHP > bot.Damage) //If its not the final blow
                    lastLog = _fightLogService.CreateFightLog(
                         new DataAccessLayer.Models.FightLog
                         {
                             FightId = fight.Id,
                             PlayerHitPoint = lastLog.PlayerHitPoint - bot.Damage,
                             BotHitPoint = lastLog.BotHitPoint,
                             Turn = lastLog.Turn + 1,
                             LogEntry = String.Format("Bot {0}: Dealt {1} damage to the Player {2}.", bot.Id, bot.Damage, player.Id)
                         });
                else
                    lastLog = _fightLogService.CreateFightLog(
                        new DataAccessLayer.Models.FightLog
                        {
                            FightId = fight.Id,
                            PlayerHitPoint = 0,
                            BotHitPoint = lastLog.BotHitPoint,
                            Turn = lastLog.Turn + 1,
                            LogEntry = String.Format("Bot {0}: Dealt {1} damage to the Player {2}. Bot won!", bot.Id, lastLog.PlayerHitPoint, player.Id)
                        });

            }
            else
            {
                lastLog = _fightLogService.CreateFightLog(
                    new DataAccessLayer.Models.FightLog
                    {
                        FightId = fight.Id,
                        PlayerHitPoint = lastLog.PlayerHitPoint,
                        BotHitPoint = lastLog.BotHitPoint,
                        Turn = lastLog.Turn + 1,
                        LogEntry = String.Format("Bot {0}: Couldn't exceed the armor class.", bot.Id)
                    });
            }
        }

        private bool RollAttack(int ArmorClass)
        {
            Random rnd = new Random();

            return rnd.Next(1, 21) >= ArmorClass;
        }

    }
}
