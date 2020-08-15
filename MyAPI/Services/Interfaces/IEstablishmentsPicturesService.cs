using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DTOs;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsPicturesService
    {
        PicturesDTO GetLogo(int estabId);
        PicturesDTO GetPicture(string picId);
        EstablishmentPicturesEditionVwMdl GetCurrentPictures(int estabId);
        void EditPictures(EstablishmentPicturesEditionVwMdl estabPics);
        void DeletePictures(int estabId);
    }
}
