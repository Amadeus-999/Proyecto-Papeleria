using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
using Microsoft.Data.SqlClient;
namespace proyecto.Admin.Function;

public partial class EditUsuarioPage : ContentPage
{
    public Usuario Usuario { get; set; }
    private readonly Action _onUsuarioEdited;
    private readonly DatabaseService _databaseService = new DatabaseService();
    public EditUsuarioPage(Usuario usuario, Action onUsuarioEdited)
	{
		InitializeComponent();
        Usuario = usuario;
        BindingContext = this;
        _onUsuarioEdited = onUsuarioEdited;
    }
    private async void OnGuardarCambiosClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Usuario.Nombre) || string.IsNullOrEmpty(Usuario.Apellido) ||
            string.IsNullOrEmpty(Usuario.Correo) || string.IsNullOrEmpty(Usuario.Contrasena))
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
                        UPDATE usuarios
                        SET nombre = @Nombre, apellido = @Apellido, correo = @Correo, contrasena = @Contrasena, rol = @Rol
                        WHERE id_usuario = @IdUsuario";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", Usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", Usuario.Apellido);
                command.Parameters.AddWithValue("@Correo", Usuario.Correo);
                command.Parameters.AddWithValue("@Contrasena", Usuario.Contrasena);
                command.Parameters.AddWithValue("@Rol", Usuario.Rol);
                command.Parameters.AddWithValue("@IdUsuario", Usuario.IdUsuario);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Usuario actualizado correctamente.", "OK");

                _onUsuarioEdited?.Invoke();
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar usuario: {ex.Message}", "OK");
        }
    }
}