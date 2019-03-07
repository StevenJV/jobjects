using System;
using System.Net.Http;

namespace swapi
{
    class Program
    {
        static string _url = "http://swapi.co/api/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello Galaxy!");
            string planets = GetPlanet(1);
            Console.WriteLine(planets);

            Console.ReadKey();
        }


        public static string GetPlanet(int planetId)
        {
            using (var httpClient = new HttpClient())
            {
                string fullUrl = _url + "planets/" + planetId;
                var response = httpClient.GetStringAsync(new Uri(fullUrl)).Result;
                return response;
            }
        }
        public static string GetPlanet()
        {
            using (var httpClient = new HttpClient())
            {
                string fullUrl = _url + "planets/";
                var response = httpClient.GetStringAsync(new Uri(fullUrl)).Result;
                return response;
            }
        }
    }
}
