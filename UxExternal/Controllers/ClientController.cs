using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using UxExternal.Models;
using Kendo.Mvc.Extensions;
using System.Net;
using System.Net.Http;

namespace UxExternal.Controllers
{
    public class ClientController : Controller
    {
        private string baseURL = @"https://building-api.azurewebsites.net";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddClient([FromBody] CreateClientRequestDto clientData)
        {
            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.PostAsJsonAsync(
            $"{baseURL}/clients/", clientData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateClient([FromBody] ClientDto clientData)
        {
            HttpClient client = new HttpClient();


            HttpResponseMessage response = await client.PutAsJsonAsync(
            $"{baseURL}/clients/{clientData.id}", clientData);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.


            return Json(null);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteClient([FromBody] ClientDto clientDto)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync(
            $"{baseURL}/clients/{clientDto.id}");
            
            // return URI of the created resource.


            return Json(clientDto.id);
        }
        [HttpPost]
        public async Task<ActionResult> GetClient([FromBody] ClientDto clientDto)
        {
            HttpClient client = new HttpClient();

            ClientDto resClient = null;
            HttpResponseMessage response = await client.GetAsync($"{baseURL}/clients/{clientDto.id}");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                resClient = await response.Content.ReadAsAsync<ClientDto>();
            }

            // return URI of the created resource.


            return Json(resClient);
        }
        public async Task<ActionResult> Clients_Read([DataSourceRequest]DataSourceRequest request)
        {

            HttpClient client = new HttpClient();

            List<ClientDto> clients = null;

            HttpResponseMessage response = await client.GetAsync($"{baseURL}/clients/");
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                clients = await response.Content.ReadAsAsync<List<ClientDto>>();
            }

            var dsResult = clients.ToDataSourceResult(request);
            return Json(dsResult);
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Client_Create([DataSourceRequest] DataSourceRequest request, CreateClientRequestDto clientData)
        {
            HttpClient client = new HttpClient();

            if (clientData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                $"{baseURL}/clients/", clientData);
                response.EnsureSuccessStatusCode();

                // return URI of the created resource.

            }

            return Json(new[] { clientData }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Client_Update([DataSourceRequest] DataSourceRequest request, ClientDto clientData)
        {
            ClientDto result = null;

            HttpClient client = new HttpClient();
            if (clientData != null && ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                $"{baseURL}/clients/{clientData.id}", clientData);
                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                result = await response.Content.ReadAsAsync<ClientDto>();
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs("Post")]
        public async Task<ActionResult> Client_Destroy([DataSourceRequest] DataSourceRequest request, ClientDto clientData)
        {
            HttpClient client = new HttpClient();

            if (clientData != null)
            {
                HttpResponseMessage response = await client.DeleteAsync(
               $"{baseURL}/clients/{clientData.id}");
            }

            return Json(new[] { clientData }.ToDataSourceResult(request, ModelState));
        }
    }
}
