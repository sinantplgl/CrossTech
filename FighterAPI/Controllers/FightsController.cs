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
        private IAbilityService _abilityService;
        private static Random rnd;
        public FightsController(
            IFightService fightService, 
            IPlayerService playerService, 
            IFightLogService fightLogService,
            IAbilityService abilityService)
        {
            _fightService = fightService;
            _playerService = playerService;
            _fightLogService = fightLogService;
            _abilityService = abilityService;

            rnd = new Random();
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
            var botId = Guid.Parse("b5774a2a-95da-e911-a603-80fa5b0fc197");
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
                        Turn = l.Turn,
                        PlayerHitPoint = l.PlayerHitPoint,
                        BotHitPoint = l.BotHitPoint,
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
            
            //Check if the fight didn't end yet
            if (lastLog.PlayerHitPoint != 0 && lastLog.BotHitPoint != 0)
            {
                //Players makes a regular attack
                if (RollAttack(bot.ArmorClass)) //Attack hits
                {
                    int currentHP = lastLog.BotHitPoint;
                    if (currentHP > player.Damage) //If its not the final blow
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
                    lastLog = _fightLogService.CreateFightLog(
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
                        Turn = l.Turn,
                        PlayerHitPoint = l.PlayerHitPoint,
                        BotHitPoint = l.BotHitPoint,
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
            var ability = _abilityService.GetAbility(abilityId);
            if (ability == null)
                return NotFound();

            //Check if the ability belongs to player
           else if (ability.PlayerId != player.Id)
                return RegularAttack(id);

            //Check if the fight didn't end yet
            else if(lastLog.PlayerHitPoint != 0 && lastLog.BotHitPoint != 0)
            {
                //Roll for an attack
                if(RollAttack(bot.ArmorClass)) //If hits
                {
                    if(lastLog.BotHitPoint > ability.Damage) //If its not final blow
                    {
                        lastLog = _fightLogService.CreateFightLog(
                            new DataAccessLayer.Models.FightLog
                            {
                                FightId = fight.Id,
                                PlayerHitPoint = lastLog.PlayerHitPoint,
                                BotHitPoint = lastLog.BotHitPoint - ability.Damage,
                                Turn = lastLog.Turn + 1,
                                LogEntry = String.Format("Player {0}: Used an ability({1}). Dealt {2} damage to the Bot {3}.", player.Id, ability.Name, ability.Damage, bot.Id)
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
                                LogEntry = String.Format("Player {0}: Used an ability({1}). Dealt {2} damage to the Bot {3}. Player won!", player.Id, ability.Name, lastLog.BotHitPoint, bot.Id)
                            });
                    }
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
                            LogEntry = String.Format("Player {0}: Used an ability({1}). Couldn't exceed the armor class.", player.Id, ability.Name)
                        });
                    //Bot makes an attack
                    BotAttack(fight, player, bot, lastLog);
                }
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
                        Turn = l.Turn,
                        PlayerHitPoint = l.PlayerHitPoint,
                        BotHitPoint = l.BotHitPoint,
                        LogEntry = l.LogEntry
                    }).OrderByDescending(log => log.Turn)
            });
        }

        private void BotAttack(
            DataAccessLayer.Models.Fight fight, DataAccessLayer.Models.Player player,
            DataAccessLayer.Models.Player bot, DataAccessLayer.Models.FightLog lastLog)
        {
            string abilityText = "";
            int damage = bot.Damage;
            if (bot.Abilities.Count() > 0 && rnd.Next(100) < 20) //It has 20% chance by default to use its speacial attack
            {
                var ability = bot.Abilities.ElementAt(rnd.Next(bot.Abilities.Count()));
                damage = ability.Damage;
                abilityText = String.Format("Used an ability({0}). ", ability.Name);
            }
            //Bot rolls for an attack
            if(RollAttack(player.ArmorClass))
            {
                int currentHP = lastLog.PlayerHitPoint;

                if (currentHP > damage) //If its not the final blow
                    lastLog = _fightLogService.CreateFightLog(
                         new DataAccessLayer.Models.FightLog
                         {
                             FightId = fight.Id,
                             PlayerHitPoint = lastLog.PlayerHitPoint - damage,
                             BotHitPoint = lastLog.BotHitPoint,
                             Turn = lastLog.Turn + 1,
                             LogEntry = String.Format("Bot {0}: {3}Dealt {1} damage to the Player {2}.", bot.Id, damage, player.Id, abilityText)
                         });
                else
                    lastLog = _fightLogService.CreateFightLog(
                        new DataAccessLayer.Models.FightLog
                        {
                            FightId = fight.Id,
                            PlayerHitPoint = 0,
                            BotHitPoint = lastLog.BotHitPoint,
                            Turn = lastLog.Turn + 1,
                            LogEntry = String.Format("Bot {0}: {3}Dealt {1} damage to the Player {2}. Bot won!", bot.Id, lastLog.PlayerHitPoint, player.Id, abilityText)
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
                        LogEntry = String.Format("Bot {0}: {1}Couldn't exceed the armor class.", bot.Id, abilityText)
                    });
            }
        }

        private bool RollAttack(int ArmorClass)
        {
            var i = rnd.Next(1000) % 20;
            return (i == 0 ? i + 20 : i + 1) >= ArmorClass;
        }

    }
}
