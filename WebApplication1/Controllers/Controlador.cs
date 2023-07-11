using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("controlador")]
    public class Controlador : ControllerBase
    {
        private static readonly string[] nombres = new[]
        {
            "pepe", "jacobo", "ruperta"
        };
        private static List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

        [HttpGet]
        public IActionResult Get()
        {
            var ran = new Random();
            for (int i = 0; i < 5; i++)
            {
                var forecast = new WeatherForecast
                {
                    Nombre = nombres[ran.Next(nombres.Length)],
                    Edad = ran.Next(15,25),
                    Fecha = DateTime.Now.AddDays(i),
                    Id = ran.Next(0,5)
                };
                weatherForecasts.Add(forecast);
            }

            if (weatherForecasts.Count > 0)
                return Ok(weatherForecasts); // Devuelve código de estado 200
            else
                return NoContent(); // No hay contenido, devuelve codigo de estado 204
        }
        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast newForecast)
        {
            // id random
            int newForecastId = new Random().Next(1000);

            // Establecer el URI del nuevo recurso.
            string newResourceUri = $"/weatherforecast/{newForecastId}";

            newForecast.Id = newForecastId;
            weatherForecasts.Add(newForecast);

            return Created(newResourceUri, newForecast);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var forecast = weatherForecasts.FirstOrDefault(f => f.Id == id);

            if (forecast != null)
                return Ok(forecast); // Devuelve, codigo de estado 200
            else
                return NotFound(); // No existe, codigo de estado 404
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WeatherForecast updatedForecast)
        {
            var existingForecast = weatherForecasts.FirstOrDefault(f => f.Id == id);

            if (existingForecast == null)
                return NotFound(); // No existe, codigo de estado 404

            if (existingForecast.Id != updatedForecast.Id)
                return Conflict("La solicitud no coincide con el ID de la ruta."); // ID en conflicto (código de estado HTTP 409).

            existingForecast.Fecha = updatedForecast.Fecha;
            existingForecast.Edad = updatedForecast.Edad;
            existingForecast.Nombre = updatedForecast.Nombre;

            return NoContent(); // Datos actualizados, código de estado204
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingForecast = weatherForecasts.FirstOrDefault(f => f.Id == id);

            if (existingForecast == null)
                return NotFound(); // No existe, codigo de estado 404

            weatherForecasts.Remove(existingForecast);

            return NoContent(); // Eliminado, codigo 204
        }

    }

    public class WeatherForecast
    {
        public DateTime Fecha { get; set; }
        public int Edad { get; set; }
        public string Nombre { get; set; }
        public int Id { get; set; }
    }
}

