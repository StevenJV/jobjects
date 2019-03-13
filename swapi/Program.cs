using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace StarWarsAPI
{
    public class Program
    {
        private const string Url = "http://swapi.co/api/";
        private static readonly HttpClient HttpClient = new HttpClient();

        private static void Main(string[] args)
        {
            var countofplanets = GetNumberOfPlanets();
            Console.WriteLine($"Hello Galaxy! There are {countofplanets} planets in the Star Wars universe. These have been seen in the movies:");
            for (var planetNumber = 1;
                planetNumber <= countofplanets;
                planetNumber++)
            {
                var planetInfo = JObject.Parse(GetPlanet(planetNumber));
                var planetName = planetInfo.Value<string>("name");
                var planetTerrain = planetInfo.Value<string>("terrain");

                var planetFilms = planetInfo["films"].Values<string>().ToList();
                if (!planetFilms.Any()) continue;
                Console.WriteLine($"{planetName} ({planetTerrain})");
                Console.WriteLine(" seen in:");
                foreach (var film in planetFilms)
                {
                    var uriAddress1 = new Uri(film);
                    if (!int.TryParse(uriAddress1.Segments[uriAddress1.Segments.Length - 1].TrimEnd('/'),
                        out var filmNumber)) continue;
                    var filmInfo = JObject.Parse(GetFilm(filmNumber));
                    var filmName = filmInfo.Value<string>("title");
                    Console.WriteLine($"  {filmName}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("No Bothans died to bring us this information.");
            Console.ReadKey();
        }

        private static string GetFilm(int filmNumber)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var fullUrl = Url + "films/" + filmNumber;
            var response = HttpClient.GetStringAsync(new Uri(fullUrl)).Result;
            return response;
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