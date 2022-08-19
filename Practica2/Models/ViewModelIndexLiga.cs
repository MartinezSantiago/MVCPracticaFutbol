using Practica2.Mapper.DTOs;

namespace Practica2.Models
{
    public class ViewModelIndexLiga
    { 
         public IEnumerable<LigaEditDto> Ligas{get;set;}
            public string Country { get; set; }
    }
}
