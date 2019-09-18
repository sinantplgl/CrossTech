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
            throw new NotImplementedException();
        }

        public void DeletePlayer(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.Include(p => p.Abilities).ToList();
        }

        public Player GetPlayer(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
