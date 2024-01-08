using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI2.Models
{
    public class EditRestaurantDTO
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
    }
}
