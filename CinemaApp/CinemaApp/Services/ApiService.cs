using CinemaApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static System.Net.WebRequestMethods;

namespace CinemaApp.Services
{
    internal class ApiService
    {
        private const string AccessTokenKey = "accessToken";
        private HttpClient _httpClient;
        private string _apiBaseUrl = $"{AppSettings.BaseUrl}/api";
        public async Task<bool> RegisterAsync(string name, string email, string password)
        {
            var registerationRequest = new RegisterationRequest(name, email, password);
            _httpClient = new HttpClient();
            var requestUri = $"{_apiBaseUrl}/users/register";
            var response = await _httpClient.PostAsync(requestUri, JsonContent.Create(registerationRequest));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequest(email, password);
            _httpClient = new HttpClient();
            string requestUri = $"{_apiBaseUrl}/users/login";
            var response = await _httpClient.PostAsync(requestUri, JsonContent.Create(loginRequest));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var token = await response.Content.ReadFromJsonAsync<Token>();
            Preferences.Set(AccessTokenKey, token.access_token);
            Preferences.Set("userId", token.user_id);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(int page, int size)
        {
            var url = GetMoviesUrl(page, size);
            var accessToken = Preferences.Get(AccessTokenKey, string.Empty);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access Token is empty");
            }

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movies = await response.Content.ReadFromJsonAsync<IEnumerable<Movie>>();
                return movies;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Movie>();
                throw;
            }
        }

        public async Task<MovieDetail> GetMovieDetail(int movieId)
        {
            var url = $"{_apiBaseUrl}/movies/MovieDetail/{movieId}";
            var accessToken = Preferences.Get(AccessTokenKey, string.Empty);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access Token is empty");
            }
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var movieDetail = await response.Content.ReadFromJsonAsync<MovieDetail>();
            return movieDetail;
        }

        public async Task<IEnumerable<FindMovieResult>> FindMovie(string searchTerm)
        {
            var url = $"{_apiBaseUrl}/movies/FindMovies?movieName={searchTerm}";
            var accessToken = Preferences.Get(AccessTokenKey, string.Empty);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access Token is empty");
            }
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var searchResult = await response.Content.ReadFromJsonAsync<IEnumerable<FindMovieResult>>();
            return searchResult;
        }

        public async Task<bool> ReserveMovie(Reservation reservation)
        {
            var url = $"{_apiBaseUrl}/reservations";
            var accessToken = Preferences.Get(AccessTokenKey, string.Empty);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access Token is empty");
            }
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await _httpClient.PostAsync(url, JsonContent.Create(reservation));
            return response.IsSuccessStatusCode;
        }

        private string GetMoviesUrl(int page, int size)
        {
            return $"{_apiBaseUrl}/movies/AllMovies?sort=asc&pageNumber={page}&pageSize={size}";
        }
    }
}
