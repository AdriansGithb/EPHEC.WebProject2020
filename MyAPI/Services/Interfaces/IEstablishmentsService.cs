using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsService
    {
        string Create(Establishments newEstab);
        EstablishmentEditionVwMdl GetEstablishment(int estabId);
        List<EstablishmentShortVwMdl> GetAllNotValidated(); 
        List<EstablishmentShortVwMdl> GetAllValidated();
        List<EstablishmentShortVwMdl> GetAllByManager(string managerId);
        EstablishmentFullVwMdl GetDetails(int estabId);
        void Delete(int estabId);
        void Validate(int estabId);
        void Edit(EstablishmentEditionVwMdl editedEstab);
        ShortenUrlVwMdl GenerateShortUrl(int estabId);
        ShortenUrlVwMdl GetShortUrl(int estabId);
    }
}
