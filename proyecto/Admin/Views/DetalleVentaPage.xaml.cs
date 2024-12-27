using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proyecto.Admin.Views;

public partial class DetalleVentaPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    private readonly int _idVenta;
    public DetalleVentaPage(int idVenta)
	{
		InitializeComponent();
        _idVenta = idVenta;
        CargarDetallesVenta();
    }
    private async void CargarDetallesVenta()
    {
        try
        {
            List<DetalleVenta> detalles = new List<DetalleVenta>();
                        
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        SELECT dv.id_producto, p.nombre_producto, dv.cantidad, dv.precio_unitario
                        FROM detalle_ventas dv
                        INNER JOIN productos p ON dv.id_producto = p.id_producto
                        WHERE dv.id_venta = @IdVenta";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdVenta", _idVenta);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        detalles.Add(new DetalleVenta
                        {
                            IdProducto = reader.GetInt32(0),
                            NombreProducto = reader.GetString(1),
                            Cantidad = reader.GetInt32(2),
                            PrecioUnitario = reader.GetDecimal(3)
                        });
                    }
                }
            }

            DetalleVentaCollectionView.ItemsSource = detalles;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar detalles de venta: {ex.Message}", "OK");
        }
    }
}