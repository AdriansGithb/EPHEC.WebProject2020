using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

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

        public PicturesDTO GetPicture(string picId)
        {
            try
            {
                EstablishmentsPictures pic = _context.EstablishmentsPictures
                    .FirstOrDefault(x => x.Id == picId);

                if (picId == null)
                {
                    return new PicturesDTO();
                }

                return new PicturesDTO
                {
                    PictureAsArrayBytes = pic.Picture
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EstablishmentPicturesEditionVwMdl GetCurrentPictures(int estabId)
        {
            try
            {
                var pictures = _context.EstablishmentsPictures
                    .Where(x => x.EstablishmentId == estabId).ToList();

                if (pictures.Count==0)
                {
                    return new EstablishmentPicturesEditionVwMdl
                    {
                        NewPictures = new List<PicturesEditionVwMdl>(),
                        CurrentPicturesId = new List<string>(),
                        EstabId = estabId
                    };
                }

                EstablishmentPicturesEditionVwMdl estabPictures = new EstablishmentPicturesEditionVwMdl
                {
                    EstabId = estabId,
                    CurrentPicturesId = new List<string>(),
                    NewPictures = new List<PicturesEditionVwMdl>()
                };

                foreach (var pic in pictures)
                {
                    if (pic.IsLogo)
                        estabPictures.CurrentLogoId = pic.Id;
                    else
                        estabPictures.CurrentPicturesId.Add(pic.Id);
                }



                return estabPictures;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void EditPictures(EstablishmentPicturesEditionVwMdl estabPics)
        {
            try
            {
                var dbEstabPics = _context.EstablishmentsPictures
                    .Where(x => x.EstablishmentId == estabPics.EstabId).ToList();

                foreach (var newPic in estabPics.NewPictures)
                {
                    if (newPic.IsLogo && dbEstabPics.Exists(x => x.IsLogo == true))
                    {
                        int indexDbLogo = dbEstabPics.FindIndex(z => z.IsLogo == true);
                        dbEstabPics[indexDbLogo].Picture = newPic.PictureAsArray;
                        _context.EstablishmentsPictures.Update(dbEstabPics[indexDbLogo]);
                    }
                    else
                    {
                        EstablishmentsPictures addPic = new EstablishmentsPictures
                        {
                            Id = null,
                            EstablishmentId = estabPics.EstabId,
                            Establishment = null,
                            Picture = newPic.PictureAsArray,
                            IsLogo = newPic.IsLogo
                        };
                        _context.EstablishmentsPictures.Add(addPic);
                    }
                }

                var estab = _context.Establishments.First(x => x.Id == estabPics.EstabId);
                estab.IsValidated = false;
                _context.Establishments.Update(estab);

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletePictures(int estabId)
        {
            try
            {
                var picsToDelete = _context.EstablishmentsPictures.Where(x => x.EstablishmentId == estabId).ToList();

                _context.RemoveRange(picsToDelete);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
