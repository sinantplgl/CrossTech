using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer
{
    public class PlayerService : IPlayerService
    {
        private CrossTechFighterContext _context;

        public PlayerService(CrossTechFighterContext context)
        {
            _context = context;
        }

        public Player CreatePlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();

            return player;
        }

        public void DeletePlayer(Player player)
        {
            _context.Remove(player);
            _context.SaveChanges();
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.Include(p => p.Abilities).ToList();
        }

        public Player GetPlayer(Guid id)
        {
            return _context.Players
                .Include(p => p.Abilities)
                .SingleOrDefault(p => p.Id == id);
   
        }

        public void UpdatePlayer(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
