using proyecto.Admin.Views;
using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Collections.Generic;

namespace proyecto.Admin.Function;

public partial class AddPedidoPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();

    public AddPedidoPage()
	{
		InitializeComponent();
        CargarProveedores();
    }
    private async void CargarProveedores()
    {
        try
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT id_proveedor, nombre_proveedor FROM proveedores";
                var command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new Proveedor
                        {
                            IdProveedor = reader.GetInt32(0),
                            NombreProveedor = reader.GetString(1)
                        });
                    }
                }
            }

            ProveedorPicker.ItemsSource = proveedores;
            ProveedorPicker.SelectedItem = proveedores.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar proveedores: {ex.Message}", "OK");
        }
    }
    private async void OnGuardarPedidoClicked(object sender, EventArgs e)
    {
        if (ProveedorPicker.SelectedItem == null || string.IsNullOrEmpty(TotalEntry.Text))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        var proveedorSeleccionado = ProveedorPicker.SelectedItem as Proveedor;
        decimal totalPedido;
        if (!decimal.TryParse(TotalEntry.Text, out totalPedido))
        {
            await DisplayAlert("Error", "El total del pedido debe ser un número válido.", "OK");
            return;
        }

        var nuevoPedido = new Pedidos
        {
            IdProveedor = proveedorSeleccionado.IdProveedor,
            FechaPedido = FechaPedidoPicker.Date,
            Estado = EstadoPicker.SelectedItem.ToString(),
            Total = totalPedido
        };

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        INSERT INTO pedidos (id_proveedor, fecha_pedido, estado, total)
                        VALUES (@IdProveedor, @FechaPedido, @Estado, @Total)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProveedor", nuevoPedido.IdProveedor);
                command.Parameters.AddWithValue("@FechaPedido", nuevoPedido.FechaPedido);
                command.Parameters.AddWithValue("@Estado", nuevoPedido.Estado);
                command.Parameters.AddWithValue("@Total", nuevoPedido.Total);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Pedido agregado correctamente.", "OK");

                if (Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as GestionPedidosPage;
                    await Navigation.PopAsync();
                    previousPage?.RecargarPedidos();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al agregar el pedido: {ex.Message}", "OK");
        }
    }
}