using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APIProject_MovieAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace APIProject_MovieAPI.Controllers
{
    public class MovieController : Controller
    {
        private readonly CinemaContext _context;
        private readonly IConfiguration _configuration;

        public MovieController(CinemaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SearchByTitle(string title, int year)
        {
            string yearstring = year.ToString();
            if (year != 0)
            {
                title.Replace(" ", "+");
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://www.omdbapi.com");

                string apiKey = _configuration.GetSection("AppConfiguration")["APIKey"];
                var response = await client.GetAsync($"?t={title}&y={yearstring}&apikey={apiKey}");
                var name = await response.Content.ReadAsAsync<Movie>();
                return View(name);
            }
            else
            {
                title.Replace(" ", "+");
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://www.omdbapi.com");
                string apiKey = _configuration.GetSection("AppConfiguration")["APIKey"];

                var response = await client.GetAsync($"?t={title}&apikey={apiKey}");
                var name = await response.Content.ReadAsAsync<Movie>();
                return View(name);
            }
        }
        public IActionResult AddFavorite(Movie movie)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            FavoriteMovies favorite = new FavoriteMovies();

            if (ModelState.IsValid)
            {
                favorite.UserId = thisUser.Id;
                favorite.Director = movie.Director;
                favorite.Rated = movie.Rated;
                favorite.Runtime = movie.Runtime;
                favorite.Title = movie.Title;
                favorite.Year = movie.Year;
                favorite.Genre = movie.Genre;

                _context.FavoriteMovies.Add(favorite);
                _context.SaveChanges();
                return RedirectToAction("FavoriteMovies");
            }
           return RedirectToAction("Index");
        }
    public IActionResult FavoriteMovies()
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteMovies> favoriteList = _context.FavoriteMovies.Where(u => u.UserId == thisUser.Id).ToList();
            return View(favoriteList);
        }

        public IActionResult DeleteFavorite(FavoriteMovies movie)
        {
            if(movie != null)
            {
                _context.FavoriteMovies.Remove(movie);
                _context.SaveChanges();
                return RedirectToAction("FavoriteMovies");
            }
            return RedirectToAction("FavoriteMovies");
        }
    }
}