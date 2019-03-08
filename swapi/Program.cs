using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace swapi
{
    class Program
    {
        static string _url = "http://swapi.co/api/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello Galaxy!");
            var planetData = JObject.Parse(GetPlanet());
            var planetNames = from p in planetData["results"] select (string) p["name"]; // ok for GetPlanet(), but not for GetPlanet(1)
            foreach(string name in planetNames) Console.WriteLine(name);
            Console.ReadKey();
        }


        public static string GetPlanet(int planetId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string fullUrl = _url + "planets/" + planetId;
                var response = httpClient.GetStringAsync(new Uri(fullUrl)).Result;
                return response;
            }
        }
        public static string GetPlanet()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string fullUrl = _url + "planets/";
                var response = httpClient.GetStringAsync(new Uri(fullUrl)).Result;
                return response;
            }
        }
    }
}
