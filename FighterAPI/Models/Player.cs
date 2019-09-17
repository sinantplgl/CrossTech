using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FighterAPI.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int HitPoint { get; set; }
        public int ArmorClass { get; set; }
        public int Damage { get; set; }

        public IEnumerable<Ability> Abilities { get; set; }
    }

    public class Ability
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
    }
}
