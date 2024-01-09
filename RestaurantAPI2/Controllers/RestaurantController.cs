using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Models;
using RestaurantAPI2.Services;

namespace RestaurantAPI2.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;
        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurantsDto = _service.GetAll();
            return StatusCode(200,restaurantsDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{Id}")]
        public ActionResult<Restaurant> Get([FromRoute] int Id)
        {
            var restaurantDto = _service.GetById(Id);
            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDTO restaurantDto)
        {
            int restaurantId = _service.Create(restaurantDto);
            return Created($"/api/restaurant/{restaurantId}",null);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete([FromRoute] int id) 
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] EditRestaurantDTO dto)
        {
            _service.Update(id, dto);
            return Ok();
        }
    }
}
