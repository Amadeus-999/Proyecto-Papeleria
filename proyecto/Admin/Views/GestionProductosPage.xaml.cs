
using proyecto.Admin.Function;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System.Collections.Generic;
namespace proyecto.Admin.Views;

public partial class GestionProductosPage : ContentPage
{
    private DatabaseService _databaseService = new DatabaseService();
    List<Producto> productos = new List<Producto>();
    public GestionProductosPage()
	{
		InitializeComponent();
        CargarProductos();
    }
    private async void CargarProductos()
    {
        List<Producto> productos = new List<Producto>();

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                SELECT p.id_producto, p.nombre_producto, p.descripcion, p.precio, p.stock, p.fecha_registro, p.ImagenUri, c.nombre_categoria
                FROM productos p
                JOIN categorias c ON p.id_categoria = c.id_categoria";  

                SqlCommand command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productos.Add(new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            NombreProducto = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Precio = reader.GetDecimal(3),
                            Stock = reader.GetInt32(4),  
                            FechaRegistro = reader.GetDateTime(5),
                            ImagenUri = reader.IsDBNull(6) ? "https://via.placeholder.com/50" : reader.GetString(6),
                            NombreCategoria = reader.GetString(7)  
                        });

                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar productos: {ex.Message}", "OK");
        }

        ProductosCollectionView.ItemsSource = productos;
    }



    private async void OnAgregarProductoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddProductoPage());
    }
    public void RecargarProductos()
    {
        CargarProductos();
    }


    private async void OnEditarProductoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        var producto = button.BindingContext as Producto;
        if (producto == null)
            return;

        await Navigation.PushAsync(new EditProductoPage(producto, CargarProductos));
    }


    private async void OnEliminarProductoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        var producto = button.BindingContext as Producto;
        if (producto == null)
            return;

        bool confirmacion = await DisplayAlert("Eliminar", $"¿Estás seguro de que quieres eliminar el producto '{producto.NombreProducto}'?", "Sí", "No");

        if (!confirmacion)
            return;

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "DELETE FROM productos WHERE id_producto = @IdProducto";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProducto", producto.IdProducto);

                    int filasAfectadas = await command.ExecuteNonQueryAsync();

                    if (filasAfectadas > 0)
                    {
                        await DisplayAlert("Éxito", "Producto eliminado correctamente.", "OK");
                        CargarProductos(); 
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el producto.", "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al eliminar el producto: {ex.Message}", "OK");
        }
    }

}