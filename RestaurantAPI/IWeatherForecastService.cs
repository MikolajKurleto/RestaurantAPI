
namespace RestaurantAPI
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get(int results, int minTemp, int maxTemp);
    }
}