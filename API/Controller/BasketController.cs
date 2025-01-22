using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entities;
using API.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{ 
     
    public class BasketController(StoreContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
             var basket = await RetrieveBasket();

            if (basket == null) return NoContent();
            return basket.ToDto();
           
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket (int productId, int quantity)
        {
            
            //get basket
            var basket = await RetrieveBasket();
            //create product
            basket ??= CreateBasket();
            //get product
            var product = await context.Products.FindAsync(productId);
            //add item to basket
            if(product ==null) return BadRequest("Problem adding item to Basket");
            basket.AddItem(product, quantity);
            //save changes
            var result = await context.SaveChangesAsync() > 0;
            if(result) return CreatedAtAction(nameof(GetBasket),basket.ToDto());
            return BadRequest("Problem updating basket");
        }

      

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem (int productId, int quantity)
        {
            //get basket
            var basket = await RetrieveBasket();
            //remove the item or reduce it quantity
            if (basket == null) return BadRequest("unable to retrieve basket");
            basket.RemoveItem(productId, quantity);
            //save changes
            var result = await context.SaveChangesAsync() >0;
            if(result) return Ok();
            return BadRequest("Problem updating basket");

        }
        
        private async Task <Basket>RetrieveBasket()
        {
            
            return await context.Baskets
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.BasketId == Request.Cookies["basketId"]);
        }

          private Basket CreateBasket()
        {
             var basketId = Guid.NewGuid().ToString();
             var cookieOptions = new CookieOptions
             {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30)
             };
             Response.Cookies.Append("basketId", basketId, cookieOptions);
             var basket = new Basket {BasketId = basketId};
             context.Baskets.Add(basket);
             return basket;

        }
    }
}