using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [Table("Players")]
    public class Player
    {
        public Player()
        {
            Abilities = new HashSet<Ability>();
        }
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int HitPoint { get; set; }
        [Required]
        public int ArmorClass { get; set; }
        [Required]
        public int Damage { get; set; }

        public virtual IEnumerable<Ability> Abilities { get; set; }
    }

    [Table("Abilities")]
    public class Ability
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Damage { get; set; }

        public virtual Player Player { get; set; }
    }
}
