using MyLibrary.Constants;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLibrary.Helpers
{
    public static class VatLayerHelper
    {
        public static async Task<bool> IsValidVatNumber(string vatNumber)
        {
            try
            {
                HttpClient client = new HttpClient();

                 var response =
                     await client.GetAsync($"{MyExternalApisConstants.MyExternalApis_VatLayer_Url}{vatNumber}");
                 if (response.IsSuccessStatusCode)
                 {
                     var content = await response.Content.ReadAsStringAsync();
                     var vatValidation = JsonConvert.DeserializeObject<VatLayerResponseModel>(content);

                     if (vatValidation.valid && vatValidation.format_valid)
                         return true;
                     else return false;

                 }
                 else return false;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
