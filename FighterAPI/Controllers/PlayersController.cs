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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/players
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/players/{id}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/players/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
