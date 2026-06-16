using System;
using Microsoft.Data.SqlClient;
using System.Text;
using Prueba_Tecnica2.Models;

namespace Prueba_Tecnica2.Data
{
    public class BibliotecaRepository
    {
        private readonly string _connectionString;

        public BibliotecaRepository(IConfiguration configuration)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "MARTINXO";
            builder.InitialCatalog = "Biblioteca";
            builder.TrustServerCertificate = true;
            builder.IntegratedSecurity = true;

            _connectionString = builder.ConnectionString;

            Console.WriteLine(_connectionString);
        }



        public LibroInfoResponse ObtenerLibro(string ISBN)
        {
            try { 
                using var conn = new SqlConnection(_connectionString);

                conn.Open();

                using var cmd = new SqlCommand("obtener_info_libro", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ISBN_reg", ISBN);

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
            catch (SqlException ex)
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
            using var conn = new SqlConnection(_connectionString);

            conn.Open();

            using var cmd = new SqlCommand("SELECT dbo.disponibilidad(@ISBN_reg)", conn);

            cmd.Parameters.AddWithValue("@ISBN_reg", ISBN);

            return cmd.ExecuteScalar()?.ToString();
        }
    }
}