using System.ComponentModel.DataAnnotations;

namespace Practica2.Mapper.DTOs
{
    public class LigaEditDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
      
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }


    }
}
