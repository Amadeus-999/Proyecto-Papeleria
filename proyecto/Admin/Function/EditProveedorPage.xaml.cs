using Microsoft.Maui.Controls;
using proyecto.Models;
using System;
using Microsoft.Data.SqlClient;

namespace proyecto.Admin.Function;

public partial class EditProveedorPage : ContentPage
{
    private readonly Proveedor _proveedor;
    private readonly Action _onProveedorEdited;
    private readonly DatabaseService _databaseService = new DatabaseService();

    public EditProveedorPage(Proveedor proveedor, Action onProveedorEdited)
	{
		InitializeComponent();
        _proveedor = proveedor;
        _onProveedorEdited = onProveedorEdited;
        BindingContext = _proveedor;
    }
    private async void OnGuardarCambiosClicked(object sender, EventArgs e)
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
                        UPDATE proveedores
                        SET nombre_proveedor = @NombreProveedor, telefono = @Telefono, correo = @Correo, direccion = @Direccion
                        WHERE id_proveedor = @IdProveedor";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProveedor", _proveedor.IdProveedor);
                command.Parameters.AddWithValue("@NombreProveedor", nombre);
                command.Parameters.AddWithValue("@Telefono", telefono);
                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Direccion", direccion ?? string.Empty);

                await command.ExecuteNonQueryAsync();

                await DisplayAlert("Éxito", "Proveedor actualizado correctamente.", "OK");

                _onProveedorEdited?.Invoke();
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar proveedor: {ex.Message}", "OK");
        }
    }
}