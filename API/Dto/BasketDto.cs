using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class BasketDto
    {
          public int Id { get; set; }
        
         public string BasketId { get; set; }

         public List<BasketItemDto> Items {get; set;} = new ();
    }
 
}