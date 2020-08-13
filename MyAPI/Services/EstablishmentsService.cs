using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public EstablishmentEditionVwMdl GetEstablishment(int estabId)
        {
            try
            {
                var estabWithRelatedEntities = _context.Establishments
                    .Where(x => x.Id == estabId)
                    .Include(x => x.Type)
                    .Include(x => x.Details)
                    .Include(x => x.Address)
                    .Include(x => x.OpeningTimes)
                    .FirstOrDefault();

                if (estabWithRelatedEntities == null)
                {
                    return new EstablishmentEditionVwMdl();
                }

                EstablishmentEditionVwMdl estab = new EstablishmentEditionVwMdl
                {
                    Id = estabWithRelatedEntities.Id,
                    Name = estabWithRelatedEntities.Name,
                    VatNum = estabWithRelatedEntities.VatNum,
                    EmailPro = estabWithRelatedEntities.Email,
                    Description = estabWithRelatedEntities.Description,
                    IsValidated = estabWithRelatedEntities.IsValidated,
                    TypeId = estabWithRelatedEntities.Type.Id,
                    Phone = estabWithRelatedEntities.Details.Phone,
                    PublicEmail = estabWithRelatedEntities.Details.Email,
                    WebsiteUrl = estabWithRelatedEntities.Details.WebsiteUrl,
                    ShortUrl = estabWithRelatedEntities.Details.ShortUrl,
                    InstagramUrl = estabWithRelatedEntities.Details.InstagramUrl,
                    FacebookUrl = estabWithRelatedEntities.Details.FacebookUrl,
                    LinkedInUrl = estabWithRelatedEntities.Details.LinkedInUrl,
                    Country = estabWithRelatedEntities.Address.Country,
                    City = estabWithRelatedEntities.Address.City,
                    ZipCode = estabWithRelatedEntities.Address.ZipCode,
                    Street = estabWithRelatedEntities.Address.Street,
                    HouseNumber = estabWithRelatedEntities.Address.HouseNumber,
                    BoxNumber = estabWithRelatedEntities.Address.BoxNumber
                };

                estab.OpeningTimesList = new List<OpeningTimesDTO>();

                foreach (var openTime in estabWithRelatedEntities.OpeningTimes.OrderBy(x=>x.DayOfWeek))
                {
                    OpeningTimesDTO oTime = new OpeningTimesDTO
                    {
                        Id = openTime.Id,
                        DayOfWeek = openTime.DayOfWeek,
                        IsOpen = openTime.IsOpen,
                        OpeningHour = openTime.OpeningHour,
                        ClosingHour = openTime.ClosingHour
                    };
                    estab.OpeningTimesList.Add(oTime);
                }

                return estab;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EstablishmentShortVwMdl> GetAllNotValidated()
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

        public List<EstablishmentShortVwMdl> GetAllValidated()
        {
            try
            {
                var estabWithTypeAndPictures = _context.Establishments
                    .Where(x => x.IsValidated == true)
                    .Include(x => x.Type)
                    .Include(x => x.Manager)
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

        public List<EstablishmentShortVwMdl> GetAllByManager(string managerId)
        {
            try
            {
                var estabWithTypeAndPictures = _context.Establishments
                    .Where(x => x.ManagerId == managerId)
                    .Include(x => x.Type)
                    .Include(x => x.Manager)
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
                    
                fullView.OpeningTimesList = new List<OpeningTimesDTO>();
                if (estabWithRelatedEntities.OpeningTimes != null)
                {
                    foreach (var openTime in estabWithRelatedEntities.OpeningTimes)
                    {
                        OpeningTimesDTO dto = new OpeningTimesDTO
                        {
                            DayOfWeek = openTime.DayOfWeek,
                            IsOpen = openTime.IsOpen,
                            OpeningHour = openTime.OpeningHour,
                            ClosingHour = openTime.ClosingHour
                        };
                        fullView.OpeningTimesList.Add(dto);
                    }
                }

                fullView.PicturesIdList = new List<string>();
                if (estabWithRelatedEntities.Pictures!=null &&
                    estabWithRelatedEntities.Pictures.Exists(x => x.IsLogo == false))
                    {
                        foreach (var picture in estabWithRelatedEntities.Pictures)
                        {
                            fullView.PicturesIdList.Add(picture.Id);
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

        public void Delete(int estabId)
        {
            try
            {
                var estab = _context.Establishments.FirstOrDefault(x=>x.Id==estabId);
                if (estab != null)
                {
                    _context.Establishments.Remove(estab);
                    _context.SaveChanges();
                }
                else throw new Exception("Establishment id not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Validate(int estabId)
        {
            try
            {
                var estab = _context.Establishments.FirstOrDefault(x => x.Id == estabId);
                if (estab != null)
                {
                    estab.IsValidated = true;
                    _context.Establishments.Update(estab);
                    _context.SaveChanges();
                }
                else throw new Exception("Establishment id not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Edit(EstablishmentEditionVwMdl editedEstab)
        {
            try
            {
                var dbEstab = _context.Establishments
                    .Where(x => x.Id == editedEstab.Id)
                    .Include(x=>x.Details)
                    .Include(x=>x.Address)
                    .Include(x=>x.OpeningTimes)
                    .FirstOrDefault();


                dbEstab.Name = editedEstab.Name;
                dbEstab.VatNum = editedEstab.VatNum;
                dbEstab.Email = editedEstab.EmailPro;
                dbEstab.Description = editedEstab.Description;
                dbEstab.IsValidated = false;
                dbEstab.TypeId = editedEstab.TypeId;

                dbEstab.Address.Country = editedEstab.Country;
                dbEstab.Address.City = editedEstab.City;
                dbEstab.Address.ZipCode = editedEstab.ZipCode;
                dbEstab.Address.Street = editedEstab.Street;
                dbEstab.Address.HouseNumber = editedEstab.HouseNumber;
                dbEstab.Address.BoxNumber = editedEstab.BoxNumber;

                dbEstab.Details.Phone = editedEstab.Phone;
                dbEstab.Details.Email = editedEstab.PublicEmail;
                dbEstab.Details.WebsiteUrl = editedEstab.WebsiteUrl;
                dbEstab.Details.ShortUrl = editedEstab.ShortUrl;
                dbEstab.Details.InstagramUrl = editedEstab.InstagramUrl;
                dbEstab.Details.FacebookUrl = editedEstab.FacebookUrl; 
                dbEstab.Details.LinkedInUrl = editedEstab.LinkedInUrl;

                for (int i = 0; i < editedEstab.OpeningTimesList.Count; i++)
                {
                    if (dbEstab.OpeningTimes.Exists(x => x.Id.Equals(editedEstab.OpeningTimesList[i].Id)))
                    {
                        int index = dbEstab.OpeningTimes.FindIndex(x =>
                            x.Id.Equals(editedEstab.OpeningTimesList[i].Id));
                        dbEstab.OpeningTimes[index].IsOpen = editedEstab.OpeningTimesList[i].IsOpen;
                        if (dbEstab.OpeningTimes[index].IsOpen)
                        {
                            dbEstab.OpeningTimes[index].OpeningHour = editedEstab.OpeningTimesList[i].OpeningHour;
                            dbEstab.OpeningTimes[index].ClosingHour = editedEstab.OpeningTimesList[i].ClosingHour;
                        }
                        else
                        {
                            dbEstab.OpeningTimes[index].OpeningHour = null;
                            dbEstab.OpeningTimes[index].ClosingHour = null;
                        }
                    }
                }

                _context.Update(dbEstab);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
