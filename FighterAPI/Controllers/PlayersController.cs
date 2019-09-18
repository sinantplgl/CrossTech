using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FighterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FighterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
        // GET: api/players
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _playerService.GetAllPlayers()
                .Select(result => new Player
                {
                    Id = result.Id,
                    Name = result.Name,
                    HitPoint = result.HitPoint,
                    ArmorClass = result.ArmorClass,
                    Damage = result.Damage,
                    Abilities = result.Abilities.Select(a => new Ability
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Damage = a.Damage
                    })
                });
        }

        // GET api/players/{id}
        [HttpGet("{id}")]
        public Player Get(Guid id)
        {
            var result = _playerService.GetPlayer(id);
            return new Player {
                Id = result.Id,
                Name = result.Name,
                HitPoint = result.HitPoint,
                ArmorClass = result.ArmorClass,
                Damage = result.Damage,
                Abilities = result.Abilities.Select(a => new Ability
                {
                    Id = a.Id,
                    Name = a.Name,
                    Damage = a.Damage
                })
            };
        }

        // POST api/players
        [HttpPost]
        public ActionResult<Player> Post([FromBody]Player player)
        {
            var result = _playerService.CreatePlayer(
                new DataAccessLayer.Models.Player
                {
                    Name = player.Name,
                    HitPoint = player.HitPoint,
                    ArmorClass = player.ArmorClass,
                    Damage = player.Damage
                });
            player.Id = result.Id;

            return Ok(player);
        }

        // PUT api/players/{id}
        [HttpPut("{id}")]
        public ActionResult<Player> Put(Guid id, [FromBody]Player player)
        {
            var result = _playerService.GetPlayer(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                result.Name = player.Name;
                result.HitPoint = player.HitPoint;
                result.ArmorClass = player.ArmorClass;
                result.Damage = player.Damage;
                _playerService.UpdatePlayer(result);
            }
            player.Id = result.Id;
            return Ok(player);
        }

        // DELETE api/players/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var result = _playerService.GetPlayer(id);
            if(result == null)
                return NotFound();
            else
                _playerService.DeletePlayer(result);

            return Ok();
        }
    }
}
