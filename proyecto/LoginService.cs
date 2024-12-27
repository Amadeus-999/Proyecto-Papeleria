using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace proyecto
{
    public class LoginService
    {
        private DatabaseService _databaseService;

        public LoginService(DatabaseService databaseService)
        {
            _databaseService = databaseService; 
        }

        public async Task<Usuario> ValidateUserAsync(string email, string password)
        {
            using (var connection = _databaseService.GetConnection())
            {
                try
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Conexión abierta con éxito.");
                    var query = "SELECT * FROM usuarios WHERE correo = @Email AND contrasena = @Password";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        Console.WriteLine($"Ejecutando consulta con email: {email} y contraseña: {password}");
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine("Usuario encontrado en la base de datos.");
                                return new Usuario
                                {
                                    IdUsuario = reader.GetInt32("id_usuario"),
                                    Nombre = reader.GetString("nombre"),
                                    Apellido = reader.GetString("apellido"),
                                    Correo = reader.GetString("correo"),
                                    Contrasena = reader.GetString("contrasena"),
                                    Rol = reader.GetString("rol"),
                                    FechaRegistro = reader.GetDateTime("fecha_registro")
                                };
                            }
                            else
                            {
                                Console.WriteLine("No se encontró al usuario con las credenciales proporcionadas.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en ValidateUserAsync: {ex.Message}");
                    throw;
                }
            }

            return null;
        }



    }
}
