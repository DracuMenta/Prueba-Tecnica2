namespace Prueba_Tecnica2.Models
{
    public class LibroInfoResponse
    {
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public string Autor { get; set; }
        public string IdAutor { get; set; }
    }
}