using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NLog.LayoutRenderers;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Models;
using RestaurantAPI2.Services;

namespace RestaurantAPI2.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Dish>> Get([FromRoute] int restaurantId)
        {
            var restaurants = _service.GetAll(restaurantId);
            return Ok(restaurants);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDTO dto)
        {
            var dishId = _service.Create(restaurantId, dto);
            return Created($"/api/restaurant/{restaurantId}/dish/{dishId}",null);
        }
    }
}
