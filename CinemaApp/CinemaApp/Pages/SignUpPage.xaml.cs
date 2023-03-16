using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private ApiService _apiService;
        public SignUpPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void ImgSignup_TappedAsync(object sender, EventArgs e)
        {
            var response = await _apiService.RegisterAsync(EntName.Text, EntEmail.Text, EntPassword.Text);
            if (response)
            {
                await DisplayAlert("Welcome", "Your registration was successful", "Ok");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await DisplayAlert("Registration Failed", "Your registration was unsuccessful", "Ok");
            }
        }

        private async void LblLogin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}