using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
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

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            var id = _service.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> Get([FromRoute] int restaurantId)
        {
            var dishes = _service.GetAll(restaurantId);

            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _service.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _service.RemoveAll(restaurantId);
            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _service.DeleteById(restaurantId, dishId);
            return NoContent();
        }
    }
}
