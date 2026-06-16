using MySql.Data.MySqlClient;
using Prueba_Tecnica2.Models;

namespace Prueba_Tecnica2.Data
{
    public class BibliotecaRepository
    {
        private readonly string _connectionString;

        public BibliotecaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaDB");
        }

        public LibroInfoResponse ObtenerLibro(string ISBN)
        {
            try { 
                using var conn = new MySqlConnection(_connectionString);

                conn.Open();

                using var cmd = new MySqlCommand("obtener_info_libro", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("ISBN_reg", ISBN);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new LibroInfoResponse
                    {
                        ISBN = reader["Codigo_ISBN"].ToString(),

                        Titulo = reader["Titulo"].ToString(),

                        FechaPublicacion = Convert.ToDateTime(reader["Fecha_Publicacion"]),

                        Autor = reader["Nombre"].ToString(),

                        IdAutor = reader["ID_Autor"].ToString()
                    };
                }

                return null;
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("El libro consultado no existe"))
                {
                    return null;
                }

                throw;
            }
        }

public string ConsultarDisponibilidad(string ISBN)
        {
            using var conn = new MySqlConnection(_connectionString);

            conn.Open();

            using var cmd = new MySqlCommand("SELECT disponibilidad(@ISBN_reg)", conn);

            cmd.Parameters.AddWithValue("@ISBN_reg", ISBN);

            return cmd.ExecuteScalar()?.ToString();
        }
    }
}