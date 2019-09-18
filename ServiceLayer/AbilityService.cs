using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLayer
{
    public class AbilityService : IAbilityService
    {
        private CrossTechFighterContext _context;
        public AbilityService(CrossTechFighterContext context)
        {
            _context = context;
        }
        public Ability CreateAbility(Ability ability)
        {
            _context.Abilities.Add(ability);
            _context.SaveChanges();

            return ability;
        }

        public void DeleteAbility(Ability ability)
        {
            _context.Remove(ability);
            _context.SaveChanges();
        }

        public Ability GetAbility(Guid id)
        {
            return _context.Abilities.Find(id);
        }

        public IEnumerable<Ability> GetAllAbilities()
        {
            return _context.Abilities.ToList();
        }

        public void UpdateAbility(Ability ability)
        {
            _context.Entry(ability).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
