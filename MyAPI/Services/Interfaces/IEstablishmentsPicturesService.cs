using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DTOs;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsPicturesService
    {
        PicturesDTO GetLogo(int estabId);

    }
}
