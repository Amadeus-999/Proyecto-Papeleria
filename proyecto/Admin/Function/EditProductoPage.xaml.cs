using Microsoft.Maui.Controls;
using proyecto.Models;
using Microsoft.Data.SqlClient;

namespace proyecto.Admin.Function;

public partial class EditProductoPage : ContentPage
{
    private Producto _producto;
    private Action _onProductUpdated;
    private DatabaseService _databaseService = new DatabaseService();
    public EditProductoPage(Producto producto, Action onProductUpdated)
	{
		InitializeComponent();
        _producto = producto;
        _onProductUpdated = onProductUpdated;
        BindingContext = _producto;
    }
    private async void OnActualizarProductoClicked(object sender, EventArgs e)
    {
        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE productos 
                    SET nombre_producto = @NombreProducto, 
                        descripcion = @Descripcion, 
                        precio = @Precio, 
                        stock = @Stock, 
                        fecha_registro = @FechaRegistro, 
                        ImagenUri = @ImagenUri
                    WHERE id_producto = @IdProducto";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", _producto.IdProducto);
                    command.Parameters.AddWithValue("@NombreProducto", _producto.NombreProducto);
                    command.Parameters.AddWithValue("@Descripcion", _producto.Descripcion);
                    command.Parameters.AddWithValue("@Precio", _producto.Precio);
                    command.Parameters.AddWithValue("@Stock", _producto.Stock);
                    command.Parameters.AddWithValue("@FechaRegistro", _producto.FechaRegistro);
                    command.Parameters.AddWithValue("@ImagenUri", _producto.ImagenUri ?? "");

                    await command.ExecuteNonQueryAsync();
                }
            }

            await DisplayAlert("Éxito", "Producto actualizado correctamente.", "OK");
            _onProductUpdated?.Invoke();
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar el producto: {ex.Message}", "OK");
        }
    }
}