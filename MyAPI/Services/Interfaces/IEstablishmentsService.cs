using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsService
    {
        string Create(Establishments newEstab);
        List<EstablishmentShortVwMdl> GetAllNotValidated(); 
        List<EstablishmentShortVwMdl> GetAllValidated();
        List<EstablishmentShortVwMdl> GetAllByManager(string managerId);
        PicturesDTO GetLogo(int estabId);
        EstablishmentFullVwMdl GetDetails(int estabId);
        
    }
}
