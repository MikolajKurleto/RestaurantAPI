using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        void DeleteById(int restaurantId, int dishId);
        IEnumerable<DishDto> GetAll(int restaurantId);
        DishDto GetById(int restaurantId, int id);
        void RemoveAll(int restaurantId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public void DeleteById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId); ;

            var dish = restaurant
                .Dishes
                .FirstOrDefault(d => d.Id == dishId);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.Dishes.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return dishesDto;
        }

        public DishDto GetById(int restaurantId, int id)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == id && d.RestaurantId == restaurantId);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }
    }
}
