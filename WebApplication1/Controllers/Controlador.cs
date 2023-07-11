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
        private static List<Persona> listaPersonas = new List<Persona>();


        [HttpGet]
        public IActionResult Get()
        {
            var ran = new Random();
            for (int i = 0; i < 5; i++)
            {
                var forecast = new Persona
                {
                    Nombre = nombres[ran.Next(nombres.Length)],
                    Edad = ran.Next(15,25),
                    Fecha = DateTime.Now.AddDays(i),
                    Id = ran.Next(0,5)
                };
                listaPersonas.Add(forecast);
            }

            if (listaPersonas.Count > 0)
                return Ok(listaPersonas); // Devuelve código de estado 200
            else
                return NoContent(); // No hay contenido, devuelve codigo de estado 204
        }


        [HttpPost]
        public IActionResult Post([FromBody] Persona nuevaPersona)
        {
            int nuevaPersonaId = new Random().Next(1000);            // id random


            string newResourceUri = $"/weatherforecast/{nuevaPersonaId}";            // URI


            nuevaPersona.Id = nuevaPersonaId;
            listaPersonas.Add(nuevaPersona);

            return Created(newResourceUri, nuevaPersona);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var persona = listaPersonas.FirstOrDefault(f => f.Id == id);

            if (persona != null)
                return Ok(persona); // Devuelve, codigo de estado 200
            else
                return NotFound(); // No existe, codigo de estado 404
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Persona personaActual)
        {
            var persona = listaPersonas.FirstOrDefault(f => f.Id == id);

            if (persona == null)
                return NotFound(); // No existe, codigo de estado 404

            if (persona.Id != personaActual.Id)
                return Conflict("La solicitud no coincide con el ID de la ruta."); // ID en conflicto (código de estado HTTP 409).

            persona.Fecha = personaActual.Fecha;
            persona.Edad = personaActual.Edad;
            persona.Nombre = personaActual.Nombre;

            return NoContent(); // Datos actualizados, código de estado204
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var persona = listaPersonas.FirstOrDefault(f => f.Id == id);

            if (persona == null)
                return NotFound(); // No existe, codigo de estado 404

            listaPersonas.Remove(persona);

            return NoContent(); // Eliminado, codigo 204
        }

    }

    public class Persona
    {
        public DateTime Fecha { get; set; }
        public int Edad { get; set; }
        public string? Nombre { get; set; }
        public int Id { get; set; }
    }
}

