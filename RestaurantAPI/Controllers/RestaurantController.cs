using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateRestaurantDto restaurantToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isUpdated = _service.Update(id, restaurantToUpdate);

            return isUpdated ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _service.Delete(id);

            return isDeleted ? NoContent() : NotFound();
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = _service.Create(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurants = _service.GetAll();

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _service.GetById(id);

            if (restaurant is null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
