using proyecto.Admin.Views;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
namespace proyecto.Admin.Function;

public partial class AddUsuarioPage : ContentPage
{
    private readonly Action _onUsuarioAdded;
    private readonly DatabaseService _databaseService = new DatabaseService();
    public AddUsuarioPage()
	{
		InitializeComponent();
	}
    private async void OnGuardarUsuarioClicked(object sender, EventArgs e)
    {
        string nombre = NombreEntry.Text;
        string apellido = ApellidoEntry.Text;
        string correo = CorreoEntry.Text;
        string contrasena = ContrasenaEntry.Text;
        string rol = RolPicker.SelectedItem.ToString();

        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        INSERT INTO usuarios (nombre, apellido, correo, contrasena, rol, fecha_registro)
                        VALUES (@Nombre, @Apellido, @Correo, @Contrasena, @Rol, @FechaRegistro)";

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Apellido", apellido);
                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Contrasena", contrasena);
                command.Parameters.AddWithValue("@Rol", rol);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Usuario agregado correctamente.", "OK");

                if (Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as GestionUsuariosPage;
                    await Navigation.PopAsync();
                    previousPage?.RecargarUsuarios();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al agregar usuario: {ex.Message}", "OK");
        }
    }
}