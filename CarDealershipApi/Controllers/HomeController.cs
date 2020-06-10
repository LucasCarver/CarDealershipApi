using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarDealershipApi.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CarDealershipApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            try
            {
                ViewBag.Status = TempData["Status"].ToString();
            }
            catch (NullReferenceException) { }

            return View();
        }


        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44384/");
            return client;
        }
        public async Task<IActionResult> Search(string searchTerm, string searchType)
        {
            HttpClient client = GetClient();
            var response = await client.GetAsync("api/Vehicle");
            var result = await response.Content.ReadAsAsync<List<Vehicle>>();
            List<Vehicle> vl = new List<Vehicle>();
            if(searchType == "color")
            {
                foreach(Vehicle v in result)
                {
                    if (v.Color.Contains(searchTerm))
                    {
                        vl.Add(v);
                    }
                }
            }
            else if(searchType == "year")
            {
                foreach (Vehicle v in result)
                {
                    if (v.Year.Year == int.Parse(searchTerm))
                    {
                        vl.Add(v);
                    }
                }
            }          
            else if(searchType == "make")
            {
                foreach (Vehicle v in result)
                {
                    if (v.Make.Contains(searchTerm))
                    {
                        vl.Add(v);
                    }
                }
            }      
            else if(searchType == "model")
            {
                foreach (Vehicle v in result)
                {
                    if (v.Model.Contains(searchTerm))
                    {
                        vl.Add(v);
                    }
                }
            }
            return View(vl);
        }

        public async Task<IActionResult> ViewVehicleList()
        {
            HttpClient client = GetClient();
            var response = await client.GetAsync("api/Vehicle");
            var result = await response.Content.ReadAsAsync<List<Vehicle>>();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewIndividualVehicle(int id)
        {
            HttpClient client = GetClient();
            var response = await client.GetAsync($"api/Vehicle/{id}");
            Vehicle result;
            try
            {
                result = await response.Content.ReadAsAsync<Vehicle>();
            }
            catch (UnsupportedMediaTypeException)
            {
                TempData["Status"] = "That vehicle doesn't exist!";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ViewIndividualVehicle(Vehicle inputVehicle)
        {
            HttpClient client = GetClient();
            var response = await client.GetAsync($"api/Vehicle/{inputVehicle.Id}");
            Vehicle result;
            try
            {
                result = await response.Content.ReadAsAsync<Vehicle>();
            }
            catch (UnsupportedMediaTypeException)
            {
                TempData["Status"] = "That vehicle doesn't exist!";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        public async Task<IActionResult> AddVehicle(Vehicle newVehicle)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsJsonAsync($"api/Vehicle", newVehicle);
            var vehicleResult = await response.Content.ReadAsAsync<Vehicle>();
            return RedirectToAction("ViewIndividualVehicle", vehicleResult);
        }

        public async Task<IActionResult> UpdateVehicle(int id, Vehicle updatedVehicle)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsJsonAsync($"api/Vehicle/{id}", updatedVehicle);
            TempData["Status"] = "Vehicle updated!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteVehicle(int id)
        {
            HttpClient client = GetClient();
            await client.DeleteAsync($"api/Vehicle/{id}");
            TempData["Status"] = "Deletion Successful!";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
