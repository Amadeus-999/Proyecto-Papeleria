using proyecto.Admin.Views;
namespace proyecto.Admin;

public partial class AdminDashboardPage : ContentPage
{
	public AdminDashboardPage()
	{
		InitializeComponent();
	}
    private async void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault()?.ToString();

        switch (selectedItem)
        {
            case "Gesti�n de Productos":
                await Navigation.PushAsync(new GestionProductosPage());
                break;
            case "Gesti�n de Categor�as":
                await Navigation.PushAsync(new GestionCategoriasPage());
                break;
            case "Gesti�n de Ventas":
                await Navigation.PushAsync(new GestionVentasPage());
                break;
            case "Gesti�n de Usuarios":
                await Navigation.PushAsync(new GestionUsuariosPage());
                break;
            case "Proveedores":
                await Navigation.PushAsync(new GestionProveedoresPage());
                break;
            case "Gestion de Pedidos con Proveedores":
                await Navigation.PushAsync(new GestionPedidosPage());
                break;
            case "Dashboard":
                await Navigation.PushAsync(new DashboardPage());
                break;
        }

        ((CollectionView)sender).SelectedItem = null;
    }
}