using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Interfaces
{
    public interface IFightService
    {
        IEnumerable<Fight> GetAllFights();
        Fight GetFightById(Guid id);
        Fight CreateFight(Fight fight);
    }
}
