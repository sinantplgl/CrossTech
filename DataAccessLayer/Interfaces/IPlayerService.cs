using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayer(Guid id);
        Player CreatePlayer(Player player);
        void UpdatePlayer(Player player);
        void DeletePlayer(Guid id);
    }
}
