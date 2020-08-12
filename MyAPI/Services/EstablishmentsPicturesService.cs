using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.Entities;

namespace MyAPI.Services
{
    public class EstablishmentsPicturesService : IEstablishmentsPicturesService
    {
        private readonly AppDbContext _context;

        public EstablishmentsPicturesService(AppDbContext context)
        {
            _context = context;
        }

        public PicturesDTO GetLogo(int estabId)
        {
            try
            {
                EstablishmentsPictures logo = _context.EstablishmentsPictures
                    .FirstOrDefault(x => x.EstablishmentId == estabId && x.IsLogo == true);

                if (logo == null)
                {
                    return new PicturesDTO();
                }

                return new PicturesDTO
                {
                    PictureAsArrayBytes = logo.Picture
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
