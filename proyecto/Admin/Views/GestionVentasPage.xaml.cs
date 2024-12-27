using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace proyecto.Admin.Views;

public partial class GestionVentasPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    public GestionVentasPage()
	{
		InitializeComponent();
        CargarVentas();
    }
    private async void CargarVentas()
    {
        try
        {
            List<Venta> ventas = new List<Venta>();

            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                SELECT v.id_venta, v.id_usuario, u.nombre, v.fecha_venta, v.total 
                FROM ventas v
                INNER JOIN usuarios u ON v.id_usuario = u.id_usuario";

                var command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ventas.Add(new Venta
                        {
                            IdVenta = reader.GetInt32(0),
                            IdUsuario = reader.GetInt32(1),
                            NombreUsuario = reader.GetString(2),
                            FechaVenta = reader.GetDateTime(3),
                            Total = reader.GetDecimal(4)
                        });
                    }
                }
            }

            VentasCollectionView.ItemsSource = ventas;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar ventas: {ex.Message}", "OK");
        }
    }
    private async void OnVerDetallesVentaClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var venta = button?.BindingContext as Venta;

        if (venta != null)
        {
            await Navigation.PushAsync(new DetalleVentaPage(venta.IdVenta));
        }
    }
    private async void OnExportarCsvClicked(object sender, EventArgs e)
    {
        await ExportarVentasACsv();
    }

    private async Task ExportarVentasACsv()
    {
        try
        {
            var ventas = VentasCollectionView.ItemsSource as List<Venta>;

            if (ventas == null || ventas.Count == 0)
            {
                await DisplayAlert("Información", "No hay ventas para exportar.", "OK");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("IdVenta,IdUsuario,FechaVenta,Total");

            foreach (var venta in ventas)
            {
                sb.AppendLine($"{venta.IdVenta},{venta.IdUsuario},{venta.FechaVenta:yyyy-MM-dd},{venta.Total}");
            }

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "ventas.csv");
            File.WriteAllText(filePath, sb.ToString());

            await DisplayAlert("Éxito", $"Archivo CSV exportado a: {filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al exportar CSV: {ex.Message}", "OK");
        }
    }

}