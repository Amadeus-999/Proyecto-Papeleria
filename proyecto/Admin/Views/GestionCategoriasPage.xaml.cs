using proyecto.Admin.Function;
using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System.Collections.Generic;
namespace proyecto.Admin.Views;

public partial class GestionCategoriasPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    private List<Categoria> categorias = new List<Categoria>();
    public Action _onCategoriaAdded;
    public GestionCategoriasPage()
	{
		InitializeComponent();
        _onCategoriaAdded = CargarCategorias;
        CargarCategorias();
    }
    private async void CargarCategorias()
    {
        List<Categoria> categorias = new List<Categoria>();

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT id_categoria, nombre_categoria, descripcion, fecha_registro FROM categorias";
                SqlCommand command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categorias.Add(new Categoria
                        {
                            IdCategoria = reader.GetInt32(0),
                            NombreCategoria = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            FechaRegistro = reader.GetDateTime(3)
                        });
                    }
                }
            }

            CategoriasCollectionView.ItemsSource = categorias;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar categorías: {ex.Message}", "OK");
        }
    }

    private async void OnAgregarCategoriaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCategoriaPage());

    }
    public void RecargarCategorias()
    {
        CargarCategorias();
    }

    private async void OnEditarCategoriaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var categoria = button?.BindingContext as Categoria;
        if (categoria != null)
        {
            await Navigation.PushAsync(new EditCategoriaPage(categoria, _onCategoriaAdded));
        }
    }

    private async void OnEliminarCategoriaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var categoria = button?.BindingContext as Categoria;

        if (categoria != null)
        {
            bool confirm = await DisplayAlert(
                "Eliminar",
                $"¿Estás seguro de eliminar la categoría '{categoria.NombreCategoria}'?",
                "Sí",
                "No"
            );

            if (confirm)
            {
                try
                {
                    using (var connection = _databaseService.GetConnection())
                    {
                        await connection.OpenAsync();

                        string query = "DELETE FROM categorias WHERE id_categoria = @IdCategoria";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);

                        await command.ExecuteNonQueryAsync();

                        await DisplayAlert("Éxito", "Categoría eliminada correctamente.", "OK");

                        _onCategoriaAdded?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al eliminar categoría: {ex.Message}", "OK");
                }
            }
        }
    }
}