using Microsoft.AspNetCore.Mvc;
using Prueba_Tecnica2.Data;

namespace Prueba_Tecnica2.Controllers
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaRepository _repository;

        public LibrosController(BibliotecaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{ISBN}")]
        public IActionResult ObtenerLibro(string ISBN)
        {
            try
            {
                var libro = _repository.ObtenerLibro(ISBN);

                if (libro == null)
                {
                    return NotFound(new
                    {
                        mensaje = "El libro no existe"
                    });
                }

                return Ok(libro);
            }
            catch
            {
                return StatusCode(500,
                    new
                    {
                        mensaje = "Error al conectar con la base de datos"
                    });
            }
        }

        [HttpGet("{ISBN}/disponibilidad")]
        public IActionResult Disponibilidad(string ISBN)
        {
            try
            {
                var resultado = _repository.ConsultarDisponibilidad(ISBN);

                return Ok(new
                {
                    mensaje = resultado
                });
            }
            catch
            {
                return StatusCode(500,
                    new
                    {
                        mensaje = "Error al conectar con la base de datos"
                    });
            }
        }
    }
}