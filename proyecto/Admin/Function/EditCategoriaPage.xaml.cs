using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
namespace proyecto.Admin.Function;

public partial class EditCategoriaPage : ContentPage
{
    private readonly Categoria _categoria;
    private readonly Action _onCategoriaUpdated;
    private readonly DatabaseService _databaseService = new DatabaseService();
    public EditCategoriaPage(Categoria categoria, Action onCategoriaUpdated)
	{
		InitializeComponent();
        _categoria = categoria;
        _onCategoriaUpdated = onCategoriaUpdated;
        BindingContext = _categoria;
    }
    private async void OnActualizarCategoriaClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NombreCategoriaEntry.Text))
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
                        UPDATE categorias 
                        SET nombre_categoria = @NombreCategoria, 
                            descripcion = @Descripcion, 
                            fecha_registro = @FechaRegistro
                        WHERE id_categoria = @IdCategoria";

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreCategoria", NombreCategoriaEntry.Text);
                command.Parameters.AddWithValue("@Descripcion", DescripcionEntry.Text ?? string.Empty);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                command.Parameters.AddWithValue("@IdCategoria", _categoria.IdCategoria);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Categoría actualizada correctamente.", "OK");

                _onCategoriaUpdated?.Invoke();
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar categoría: {ex.Message}", "OK");
        }
    }
}
