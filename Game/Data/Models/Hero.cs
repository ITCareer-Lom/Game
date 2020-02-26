namespace Game.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Hero
    {
        public Hero()
        {

        }

        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Health { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Power { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Experience { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Gold { get; set; }


    }
}
