using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.Helpers;

namespace ExternalAPIs.Services.Interfaces
{
    public interface IVatLayerService
    {
        Task<VatLayerResponseModel> GetVatLayerResponseAsync(string vatNumber);
    }
}
