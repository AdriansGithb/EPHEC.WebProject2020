using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExternalAPIs.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Helpers;
using Newtonsoft.Json;

namespace ExternalAPIs.Services
{
    public class VatLayerService : IVatLayerService
    {
        private const string BaseUrl = "http://www.apilayer.net/api/validate?access_key=79a2bc10853f370daa2d40747f877d3f";
        private readonly HttpClient _client;

        public VatLayerService(HttpClient client)
        {
            _client = client;
        }

        public async Task<VatLayerResponseModel> GetVatLayerResponseAsync(string vatNumber)
        {
            try
            {
                var httpResponse = await _client.GetAsync($"{BaseUrl}&vat_number={vatNumber}");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot retrieve tasks");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<VatLayerResponseModel>(content);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
