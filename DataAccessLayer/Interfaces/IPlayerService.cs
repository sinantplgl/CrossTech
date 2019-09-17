using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetAllPlayers();
        Player GetPlayerById(Guid Id);
        Player CreatePlayer();
        void UpdatePlayer(Player player);
        void DeletePlayer(Guid id);


    }
}
