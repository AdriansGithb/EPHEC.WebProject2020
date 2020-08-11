using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services
{
    public class EstablishmentsService : IEstablishmentsService
    {
        private readonly AppDbContext _context;

        public EstablishmentsService(AppDbContext context)
        {
            _context = context;
        }

        public string Create(Establishments newEstab)
        {
            try
            {
                Establishments estabBase = new Establishments
                {
                    Name = newEstab.Name,
                    VatNum = newEstab.VatNum,
                    Email = newEstab.Email,
                    Description = newEstab.Description,
                    IsValidated = false,
                    ManagerId = newEstab.ManagerId,
                    TypeId = newEstab.TypeId,
                    Details = newEstab.Details
                };

                var createdEstab = _context.Establishments.Add(estabBase);

                newEstab.Address.Establishment = createdEstab.Entity;
                _context.Add(newEstab.Address);

                newEstab.Details.Establishment = createdEstab.Entity;
                _context.Add(newEstab.Details);

                foreach (EstablishmentsOpeningTimes schedule in newEstab.OpeningTimes)
                {
                    schedule.Establishment = createdEstab.Entity;
                    schedule.IsSpecialDay = false;
                    if (!schedule.IsOpen)
                    {
                        schedule.OpeningHour = null;
                        schedule.ClosingHour = null;
                    }
                }

                _context.AddRange(newEstab.OpeningTimes);

                if (newEstab.Pictures.Count > 0)
                {
                    foreach (EstablishmentsPictures pic in newEstab.Pictures)
                    {
                        pic.Establishment = createdEstab.Entity;
                    }
                    _context.AddRange(newEstab.Pictures);
                }
                
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public List<EstablishmentShortVwMdl> GetAllEstabNotValited()
        {
            try
            {
                var estabWithTypeAndPictures = _context.Establishments
                    .Where(x => x.IsValidated == false)
                    .Include(x=>x.Type)
                    .Include(x=>x.Manager)
                    .ToList();

                List<EstablishmentShortVwMdl> rtrnList = new List<EstablishmentShortVwMdl>();
                if (estabWithTypeAndPictures.Count > 0)
                {
                    foreach (var estab in estabWithTypeAndPictures)
                    {
                        EstablishmentShortVwMdl shortVw = new EstablishmentShortVwMdl
                        {
                            Id = estab.Id,
                            Name = estab.Name,
                            EstabType = estab.Type.Name,
                            ManagerId = estab.ManagerId,
                            ManagerUserName = estab.Manager.UserName,
                            IsValidated = estab.IsValidated
                        };

                        rtrnList.Add(shortVw);

                    }
                }

                return rtrnList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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

        public EstablishmentFullVwMdl GetDetails(int estabId)
        {
            try
            {
                var estabWithRelatedEntities = _context.Establishments
                    .Where(x => x.Id == estabId)
                    .Include(x => x.Manager)
                    .Include(x=>x.Type)
                    .Include(x => x.Details)
                    .Include(x => x.Address)
                    .Include(x => x.OpeningTimes)
                    .Include(x => x.Pictures)
                    .FirstOrDefault();

                if (estabWithRelatedEntities == null)
                {
                    return new EstablishmentFullVwMdl();
                }

                EstablishmentFullVwMdl fullView = new EstablishmentFullVwMdl
                {
                    Id = estabWithRelatedEntities.Id,
                    Name = estabWithRelatedEntities.Name,
                    VatNum = estabWithRelatedEntities.VatNum,
                    EmailPro = estabWithRelatedEntities.Email,
                    Description = estabWithRelatedEntities.Description,
                    IsValidated = estabWithRelatedEntities.IsValidated,
                    Manager = $"{estabWithRelatedEntities.Manager.LastName} {estabWithRelatedEntities.Manager.FirstName}",
                    EstabType = estabWithRelatedEntities.Type.Name,
                    Country = estabWithRelatedEntities.Address.Country,
                    City = estabWithRelatedEntities.Address.City,
                    ZipCode = estabWithRelatedEntities.Address.ZipCode,
                    Street = estabWithRelatedEntities.Address.Street,
                    HouseNumber = estabWithRelatedEntities.Address.HouseNumber,
                    BoxNumber = estabWithRelatedEntities.Address.BoxNumber,
                    Phone = estabWithRelatedEntities.Details.Phone,
                    PublicEmail = estabWithRelatedEntities.Details.Email,
                    WebsiteUrl = estabWithRelatedEntities.Details.WebsiteUrl,
                    ShortUrl = estabWithRelatedEntities.Details.ShortUrl,
                    InstagramUrl = estabWithRelatedEntities.Details.InstagramUrl,
                    FacebookUrl = estabWithRelatedEntities.Details.FacebookUrl,
                    LinkedInUrl = estabWithRelatedEntities.Details.LinkedInUrl,
                };
                    
                fullView.OpeningTimesIdList = new List<string>();
                if (estabWithRelatedEntities.OpeningTimes != null)
                {
                    foreach (var openTime in estabWithRelatedEntities.OpeningTimes)
                    {
                        fullView.OpeningTimesIdList.Add(openTime.Id);
                    }
                }

                fullView.PicturesIdList = new List<string>();
                if (estabWithRelatedEntities.Pictures!=null)
                {
                    if (estabWithRelatedEntities.Pictures.Exists(x => x.IsLogo == true))
                    {
                        fullView.LogoId = estabWithRelatedEntities.Pictures.First(x => x.IsLogo == true).Id;
                    }
                        
                    if (estabWithRelatedEntities.Pictures.Exists(x => x.IsLogo == false))
                    {                            
                        foreach (var picture in estabWithRelatedEntities.Pictures)
                        {
                            fullView.PicturesIdList.Add(picture.Id);
                        }
                    }

                }

                fullView.NewsIdList = new List<string>();
                if (estabWithRelatedEntities.News!=null)
                {
                    foreach (var news in estabWithRelatedEntities.News)
                    {
                        fullView.NewsIdList.Add(news.Id);
                    }
                }

                return fullView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
