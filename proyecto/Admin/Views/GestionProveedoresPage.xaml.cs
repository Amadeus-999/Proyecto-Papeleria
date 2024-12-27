using proyecto.Admin.Function;
using Microsoft.Maui.Controls;
using Microsoft.Data.SqlClient;
using proyecto.Models;
using System;
using System.Collections.Generic;

namespace proyecto.Admin.Views;

public partial class GestionProveedoresPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    private List<Proveedor> proveedores = new List<Proveedor>();
    public GestionProveedoresPage()
	{
		InitializeComponent();
        CargarProveedores();
    }

    private async void CargarProveedores()
    {
        List<Proveedor> proveedores = new List<Proveedor>();

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = "SELECT id_proveedor, nombre_proveedor, telefono, correo, direccion, fecha_registro FROM proveedores";
                SqlCommand command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new Proveedor
                        {
                            IdProveedor = reader.GetInt32(0),
                            NombreProveedor = reader.GetString(1),
                            Telefono = reader.GetString(2),
                            Correo = reader.GetString(3),
                            Direccion = reader.GetString(4),
                            FechaRegistro = reader.GetDateTime(5)
                        });
                    }
                }
            }

            ProveedoresCollectionView.ItemsSource = proveedores;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar proveedores: {ex.Message}", "OK");
        }
    }
    private async void OnAgregarProveedorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddProveedorPage());
    }

    public void RecargarProveedores()
    {
        CargarProveedores();
    }
    private async void OnEditarProveedorClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var proveedor = button?.BindingContext as Proveedor;
        if (proveedor != null)
        {
            await Navigation.PushAsync(new EditProveedorPage(proveedor, RecargarProveedores));
        }
    }

    private async void OnEliminarProveedorClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var proveedor = button?.BindingContext as Proveedor;

        if (proveedor != null)
        {
            bool confirm = await DisplayAlert("Eliminar", $"¿Estás seguro de eliminar el proveedor '{proveedor.NombreProveedor}'?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    using (var connection = _databaseService.GetConnection())
                    {
                        await connection.OpenAsync();

                        string query = "DELETE FROM proveedores WHERE id_proveedor = @IdProveedor";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@IdProveedor", proveedor.IdProveedor);

                        await command.ExecuteNonQueryAsync();

                        await DisplayAlert("Éxito", "Proveedor eliminado correctamente.", "OK");

                        CargarProveedores();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al eliminar proveedor: {ex.Message}", "OK");
                }
            }
        }
    }
}