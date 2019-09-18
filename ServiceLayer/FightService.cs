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
    public class FightService : IFightService
    {
        private CrossTechFighterContext _context;

        public FightService(CrossTechFighterContext context)
        {
            _context = context;
        }
        public Fight CreateFight(Fight fight)
        {
            _context.Fights.Add(fight);
            _context.SaveChanges();

            return fight;
        }

        public IEnumerable<Fight> GetAllFights()
        {
            return _context.Fights
                .Include(f => f.FightLogs)
                .ToList();
        }

        public Fight GetFightById(Guid id)
        {
            return _context.Fights
                .Include(f => f.FightLogs)
                .SingleOrDefault(f => f.Id == id);
                
        }
    }
}
