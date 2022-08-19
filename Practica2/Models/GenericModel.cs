using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Practica2.Models
{
    public class GenericModel
    {
  
        [Key]
        public int Id { get; set; }
     
        [Required]
      
        public bool IsDeleted { get; set; }
      
    [Required]
        public DateTime LastUpdated { get; set; }
    }
}
