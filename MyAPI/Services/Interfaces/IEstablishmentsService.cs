using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsService
    {
        string Create(Establishments newEstab);

        List<EstablishmentShortVwMdl> GetAllEstabNotValited();
    }
}
