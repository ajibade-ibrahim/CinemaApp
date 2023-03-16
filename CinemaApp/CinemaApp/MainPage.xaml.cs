using CinemaApp.Models;
using CinemaApp.Pages;
using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CinemaApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Movie> _moviesCollection = new ObservableCollection<Movie>();
        private ApiService _apiService = new ApiService();
        private int pageSize = 5;
        private int pageNumber = 0;

        public MainPage()
        {
            InitializeComponent();
            LoadMovies();
        }

        private async void LoadMovies()
        {
            var movies = await _apiService.GetMoviesAsync(++pageNumber, pageSize);
            movies.ForEach(movie => _moviesCollection.Add(movie));
            CvMovies.ItemsSource = _moviesCollection;
        }

        private async void TapMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private async void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private void CvMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            LoadMovies();
        }

        private void CvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault();
            if (currentSelection == null)
            {
                return;
            }

            var currentSelectedMovie = currentSelection as Movie;
            Navigation.PushAsync(new MovieDetailPage(currentSelectedMovie.Id));
            CvMovies.SelectedItem = null;
        }
    }
}
