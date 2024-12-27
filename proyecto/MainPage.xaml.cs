using proyecto.Admin;
using proyecto.User;
using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using proyecto.Models;
namespace proyecto
{
    public partial class MainPage : ContentPage
    {
        private readonly LoginService _loginService;

        public MainPage()
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _loginService = new LoginService(databaseService);

        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = emailEntry.Text;
            var password = passwordEntry.Text;

            try
            {
                var usuario = await _loginService.ValidateUserAsync(email, password);

                if (usuario != null)
                {
                    if (usuario.Rol == "admin")
                    {
                        await Navigation.PushAsync(new AdminDashboardPage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new CatalogPage());
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
                }
            }
            catch (SqlException ex)
            {
                await DisplayAlert("Error de SQL", $"Número de error: {ex.Number}\nMensaje: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }


        private void OnSignUpClicked(object sender, EventArgs e)
        {
            // Redirige a la página de registro 
        }
    }

}
