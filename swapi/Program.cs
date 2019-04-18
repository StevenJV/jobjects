using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StarWarsAPI
{
    public class Program
    {
        private const string Url = "http://swapi.co/api/";
        private static readonly HttpClient HttpClient = new HttpClient();
        private static List<string> _planetFilmNames = new List<string>();
        private static List<string> _planetFilmUrLs = new List<string>();

        private static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            var countofplanets = GetNumberOfPlanets();
            Console.WriteLine(
                $"Hello Galaxy! There are {countofplanets} planets in the Star Wars universe. These have been seen in the movies:");
            for (var planetNumber = 1;
                planetNumber <= countofplanets;
                planetNumber++)
            {
                _planetFilmUrLs.Clear();
                _planetFilmNames.Clear();
                var planetInfo = JObject.Parse(GetPlanet(planetNumber));
                var planetName = planetInfo.Value<string>("name");
                var planetTerrain = planetInfo.Value<string>("terrain");

                _planetFilmUrLs = planetInfo["films"].Values<string>().ToList();
                if (!_planetFilmUrLs.Any()) continue;
                Console.WriteLine($"{planetName} ({planetTerrain})");
                Console.WriteLine(" seen in:");
                _planetFilmNames = ListFilmsAsync(_planetFilmUrLs).Result;
                foreach (var film in _planetFilmNames)
                {
                    var filmInfo = JObject.Parse(film);
                    var filmName = filmInfo.Value<string>("title");
                    Console.WriteLine($"  {filmName}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("No Bothans died to bring us this information.");
            watch.Stop();
            Console.WriteLine($"Total execution time (jObject): {watch.ElapsedMilliseconds}");
            Console.ReadKey();
        }


        private static async Task<List<string>> ListFilmsAsync(List<string> planetFilms) //because async here
        {
            var listOfFilmTitles = new List<string>();
            foreach (var film in planetFilms)
            {
                var uriAddress1 = new Uri(film);
                if (!int.TryParse(uriAddress1.Segments[uriAddress1.Segments.Length - 1].TrimEnd('/'),
                    out var filmNumber)) continue;
                listOfFilmTitles.Add(await GetFilmInfoAsync(filmNumber)); //and await without .result here
            }

            return listOfFilmTitles; //this returns a task containing the list, not just the list
        }

        private static async Task<string> GetFilmInfoAsync(int filmNumber)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var fullUrl = Url + "films/" + filmNumber;
            return await HttpClient.GetStringAsync(new Uri(fullUrl));
        }

        public static string GetPlanet(int planetId)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var fullUrl = Url + "planets/" + planetId;
            var response = HttpClient.GetStringAsync(new Uri(fullUrl)).Result;
            return response;
        }

        public static int GetNumberOfPlanets()
        {
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var fullUrl = Url + "planets/";
            var response = HttpClient.GetStringAsync(new Uri(fullUrl)).Result;
            var data = JObject.Parse(response);
            var numberOfPlanets = (int) data.Children().First();
            return numberOfPlanets;
        }
    }
}