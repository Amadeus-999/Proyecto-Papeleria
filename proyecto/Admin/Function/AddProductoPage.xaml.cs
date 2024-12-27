using proyecto.Admin.Views;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
using System.Collections.Generic;

namespace proyecto.Admin.Function;

public partial class AddProductoPage : ContentPage
{
    public Producto NuevoProducto { get; set; } = new Producto();
    private readonly DatabaseService _databaseService = new DatabaseService();
    private List<Categoria> categorias = new List<Categoria>();
    public AddProductoPage()
	{
		InitializeComponent();
        BindingContext = NuevoProducto;
        CargarCategorias();
    }
    private async void CargarCategorias()
    {
        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT id_categoria, nombre_categoria FROM categorias";

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    categorias.Add(new Categoria
                    {
                        IdCategoria = reader.GetInt32(0),
                        NombreCategoria = reader.GetString(1)
                    });
                }

                CategoriaPicker.ItemsSource = categorias;
                CategoriaPicker.ItemDisplayBinding = new Binding("NombreCategoria");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar categorías: {ex.Message}", "OK");
        }
    }

    private async void OnGuardarProductoClicked(object sender, EventArgs e)
    {
        if (CategoriaPicker.SelectedItem is Categoria categoriaSeleccionada)
        {
            NuevoProducto.IdCategoria = categoriaSeleccionada.IdCategoria;
        }
        else
        {
            await DisplayAlert("Error", "Por favor, selecciona una categoría.", "OK");
            return;
        }

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                INSERT INTO productos (nombre_producto, descripcion, precio, stock, fecha_registro, id_categoria, ImagenUri)
                VALUES (@NombreProducto, @Descripcion, @Precio, @Stock, @FechaRegistro, @IdCategoria, @ImagenUri)";

                var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NombreProducto", NuevoProducto.NombreProducto);
                command.Parameters.AddWithValue("@Descripcion", NuevoProducto.Descripcion);
                command.Parameters.AddWithValue("@Precio", NuevoProducto.Precio);
                command.Parameters.AddWithValue("@Stock", NuevoProducto.Stock);
                command.Parameters.AddWithValue("@FechaRegistro", NuevoProducto.FechaRegistro);
                command.Parameters.AddWithValue("@IdCategoria", NuevoProducto.IdCategoria);
                command.Parameters.AddWithValue("@ImagenUri", NuevoProducto.ImagenUri);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Producto agregado correctamente.", "OK");

                if (Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as GestionProductosPage;
                    await Navigation.PopAsync();
                    previousPage?.RecargarProductos();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al agregar producto: {ex.Message}", "OK");
        }
    }

}
