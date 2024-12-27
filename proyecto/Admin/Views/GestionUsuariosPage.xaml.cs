using proyecto.Admin.Function;
using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Collections.Generic;

namespace proyecto.Admin.Views;

public partial class GestionUsuariosPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    private List<Usuario> usuarios = new List<Usuario>();
    public GestionUsuariosPage()
	{
		InitializeComponent();
        CargarUsuarios();
    }

    private async void CargarUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT id_usuario, nombre, apellido, correo, rol, fecha_registro FROM usuarios";
                SqlCommand command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Rol = reader.GetString(4),
                            FechaRegistro = reader.GetDateTime(5)
                        });
                    }
                }
            }

            UsuariosCollectionView.ItemsSource = usuarios;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar usuarios: {ex.Message}", "OK");
        }
    }

    private async void OnAgregarUsuarioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddUsuarioPage());
    }
    public void RecargarUsuarios()
    {
        CargarUsuarios();
    }

    private async void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var usuario = button?.BindingContext as Usuario;
        if (usuario != null)
        {
            await Navigation.PushAsync(new EditUsuarioPage(usuario, RecargarUsuarios));
        }
    }


    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var usuario = button?.BindingContext as Usuario;

        if (usuario != null)
        {
            bool confirm = await DisplayAlert("Eliminar", $"¿Estás seguro de eliminar al usuario '{usuario.Nombre}'?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    using (var connection = _databaseService.GetConnection())
                    {
                        await connection.OpenAsync();

                        string query = "DELETE FROM usuarios WHERE id_usuario = @IdUsuario";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                        await command.ExecuteNonQueryAsync();

                        await DisplayAlert("Éxito", "Usuario eliminado correctamente.", "OK");

                        CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al eliminar usuario: {ex.Message}", "OK");
                }
            }
        }
    }
}
