using proyecto.Admin.Views;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
namespace proyecto.Admin.Function;

public partial class AddCategoriaPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    public AddCategoriaPage()
	{
		InitializeComponent();
    }
    private async void OnGuardarCategoriaClicked(object sender, EventArgs e)
    {
        string nombre = NombreCategoriaEntry.Text;
        string descripcion = DescripcionEntry.Text;

        if (string.IsNullOrEmpty(nombre))
        {
            await DisplayAlert("Error", "El nombre de la categoría es obligatorio.", "OK");
            return;
        }

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        INSERT INTO categorias (nombre_categoria, descripcion, fecha_registro)
                        VALUES (@NombreCategoria, @Descripcion, @FechaRegistro)";

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreCategoria", nombre);
                command.Parameters.AddWithValue("@Descripcion", descripcion ?? string.Empty);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Categoría agregada correctamente.", "OK");

                if (Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as GestionCategoriasPage;
                    await Navigation.PopAsync();  
                    previousPage?.RecargarCategorias();  
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al agregar categoría: {ex.Message}", "OK");
        }
    }
}