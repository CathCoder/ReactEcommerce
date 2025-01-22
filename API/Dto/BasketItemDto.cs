using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controller.Entities;
using API.Entities;

namespace API.Dto
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public  required string Name { get; set; }
        public long Price { get; set; }
        public  required string PictureUrl { get; set; }
        public required string Brand { get; set; }
        public required string Type { get; set; }

        public int Quantity { get; set; }


    }
}