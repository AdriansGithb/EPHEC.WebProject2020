using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
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
                }

                _context.AddRange(newEstab.Pictures);

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
                    //.Include(x => x.Pictures)
                    .Include(x=>x.Type).ToList();

                List<EstablishmentShortVwMdl> rtrnList = new List<EstablishmentShortVwMdl>();
                if (estabWithTypeAndPictures.Count > 0)
                {
                    foreach (var estab in estabWithTypeAndPictures)
                    {
                        EstablishmentShortVwMdl shortVw = new EstablishmentShortVwMdl
                        {
                            Id = estab.Id,
                            Name = estab.Name,
                            EstabType = estab.Type.Name
                        };
                        //if (estab.Pictures.Count>0 && estab.Pictures.Exists(x=>x.IsLogo==true))
                        //    shortVw.LogoAsArray = estab.Pictures.First(x=>x.IsLogo==true).Picture;
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

        public EstablishmentsPictures GetLogo(int estabId)
        {
            try
            {
                EstablishmentsPictures logo = _context.EstablishmentsPictures
                    .FirstOrDefault(x => x.EstablishmentId == estabId && x.IsLogo == true);

                return logo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
