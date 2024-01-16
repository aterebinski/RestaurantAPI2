using AutoMapper;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Models;
using RestaurantAPI2.Services;
using System.Security.Claims;

namespace RestaurantAPI2.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    //[Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;
        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        //[Authorize(Policy = "AtLeast2RestaurantsCreatedByUser")]
        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll([FromQuery] RestaurantQuery query)
        {
            var restaurantsDto = _service.GetAll(query);
            return StatusCode(200,restaurantsDto);
        }

        [Authorize(Policy= "HasNationality")]
        [HttpGet("{Id}")]
        public ActionResult<Restaurant> Get([FromRoute] int Id)
        {
            var restaurantDto = _service.GetById(Id);
            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDTO restaurantDto)
        {
            var userId = int.Parse(User.FindFirst(i => i.Type == ClaimTypes.NameIdentifier).Value);
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
