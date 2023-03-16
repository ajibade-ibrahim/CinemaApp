using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private ApiService _apiService;
        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void ImgLogin_TappedAsync(object sender, EventArgs e)
        {
            var isLoggedIn = await _apiService.LoginAsync(EntEmail.Text, EntPassword.Text);
            if (isLoggedIn)
            {
                await DisplayAlert("Welcome", $"Login was successful: {Preferences.Get("accessToken", "NotFound")}", "Ok");
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Login Failed", "Login failed", "Ok");
            }
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}