using proyecto.Admin.Views;
using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
using Microsoft.Data.SqlClient;

namespace proyecto.Admin.Function;

public partial class AddProveedorPage : ContentPage
{
    private readonly Action _onProveedorAdded;
    private readonly DatabaseService _databaseService = new DatabaseService();
    public AddProveedorPage()
	{
		InitializeComponent();
	}
    private async void OnGuardarProveedorClicked(object sender, EventArgs e)
    {
        string nombre = NombreProveedorEntry.Text;
        string telefono = TelefonoEntry.Text;
        string correo = CorreoEntry.Text;
        string direccion = DireccionEntry.Text;

        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(correo))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        try
        {
            using (var connection = _databaseService.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                        INSERT INTO proveedores (nombre_proveedor, telefono, correo, direccion, fecha_registro)
                        VALUES (@NombreProveedor, @Telefono, @Correo, @Direccion, @FechaRegistro)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreProveedor", nombre);
                command.Parameters.AddWithValue("@Telefono", telefono);
                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Direccion", direccion ?? string.Empty);
                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Proveedor agregado correctamente.", "OK");

                if (Navigation.NavigationStack.Count > 1)
                {
                    var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2] as GestionProveedoresPage;
                    await Navigation.PopAsync();
                    previousPage?.RecargarProveedores();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al agregar proveedor: {ex.Message}", "OK");
        }
    }
}