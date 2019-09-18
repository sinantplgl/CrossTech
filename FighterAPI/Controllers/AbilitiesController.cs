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
    public class AbilitiesController : Controller
    {
        private IAbilityService _abilityService;

        public AbilitiesController(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        //GET: api/abilities
        [HttpGet]
        public IEnumerable<Ability> Get()
        {
            return _abilityService.GetAllAbilities()
                .Select(result => new Ability
                {
                    Id = result.Id,
                    PlayerId = result.PlayerId,
                    Name = result.Name,
                    Damage = result.Damage
                });
        }

        [HttpGet("{id}")]
        public ActionResult<Ability> Get(Guid id)
        {
            var result = _abilityService.GetAbility(id);
            return new Ability
            {
                Id = result.Id,
                PlayerId = result.PlayerId,
                Name = result.Name,
                Damage = result.Damage
            };
        }

        [HttpPost]
        public ActionResult<Ability> Post([FromBody] Ability ability)
        {
            var result = _abilityService.CreateAbility(
                new DataAccessLayer.Models.Ability
                {
                    PlayerId = ability.PlayerId,
                    Name = ability.Name,
                    Damage = ability.Damage
                });
            ability.Id = result.Id;
            return Ok(ability);
        }

        [HttpPut] ActionResult<Ability> Put(Guid id, [FromBody] Ability ability)
        {
            var result = _abilityService.GetAbility(id);
            if (result == null)
                return NotFound();
            else
            {
                result.PlayerId = ability.PlayerId;
                result.Name = ability.Name;
                result.Damage = ability.Damage;
            }
            ability.Id = result.Id;
            return Ok(ability);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var result = _abilityService.GetAbility(id);
            if (result == null)
                return NotFound();
            else
                _abilityService.DeleteAbility(result);

            return Ok();
        }
    }
}
