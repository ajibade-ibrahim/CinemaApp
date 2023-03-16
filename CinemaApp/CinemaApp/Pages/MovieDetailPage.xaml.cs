using CinemaApp.Models;
using CinemaApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CinemaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        ApiService _apiService = new ApiService();
        private MovieDetail movieDetail;
        public MovieDetailPage(int movieId)
        {
            InitializeComponent();
            GetMovieDetailAsync(movieId);
        }

        private async void GetMovieDetailAsync(int movieId)
        {
            movieDetail = await _apiService.GetMovieDetail(movieId);
            BindMovieDetail();
        }

        private void BindMovieDetail()
        {
            if (movieDetail == null)
            {
                return;
            }

            LblMovieName.Text = movieDetail.Name;
            LblDuration.Text = movieDetail.Duration;
            LblGenre.Text = movieDetail.Genre;
            LblLanguage.Text = movieDetail.Language;
            LblMovieDescription.Text = movieDetail.Description;
            LblPlayingDate.Text = movieDetail.PlayingDate.ToString("MMMM dd, yyyy");
            LblPlayingTime.Text = movieDetail.PlayingTime.ToString("HH:mm");
            LblRating.Text = movieDetail.Rating.ToString();
            LblTicketPrice.Text = movieDetail.TicketPrice.ToString("C", CultureInfo.CurrentCulture);
            ImgMovie.Source = movieDetail.FullImageUrl;
            ImgMovieCover.Source = movieDetail.FullImageUrl;
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void TapVideo_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VideoPlayerPage(movieDetail.TrailorUrl));
        }
    }
}