using proyecto.Admin.Function;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
using System.Collections.Generic;

namespace proyecto.Admin.Views;

public partial class GestionPedidosPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    private List<Pedidos> _pedidos = new List<Pedidos>();

    public GestionPedidosPage()
	{
		InitializeComponent();
        CargarPedidos();
    }
    private async void CargarPedidos()
    {
        List<Pedidos> _pedidos = new List<Pedidos>();


        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        SELECT p.id_pedido, p.fecha_pedido, p.estado, p.total, pr.id_proveedor, pr.nombre_proveedor, pr.telefono, pr.correo
                        FROM pedidos p
                        INNER JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor";

                var command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        _pedidos.Add(new Pedidos
                        {
                            IdPedido = reader.GetInt32(0),
                            FechaPedido = reader.GetDateTime(1),
                            Estado = reader.GetString(2),
                            Total = reader.GetDecimal(3),
                            Proveedor = new Proveedor
                            {
                                IdProveedor = reader.GetInt32(4),
                                NombreProveedor = reader.GetString(5),
                                Telefono = reader.GetString(6),
                                Correo = reader.GetString(7)
                            }
                        });
                    }
                }
            }

            PedidosCollectionView.ItemsSource = _pedidos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar los pedidos: {ex.Message}", "OK");
        }
    }
    private async void OnAgregarPedidoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPedidoPage());
    }

    public void RecargarPedidos()
    {
        CargarPedidos();
    }

    private async void OnVerDetallesPedidoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var pedido = button?.BindingContext as Pedidos;

        if (pedido != null)
        {
            await DisplayAlert("Detalles del Pedido", $"ID Pedido: {pedido.IdPedido}\nProveedor: {pedido.Proveedor.NombreProveedor}", "OK");
        }
    }
}