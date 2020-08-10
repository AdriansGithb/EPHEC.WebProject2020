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
            using (var transaction = _context.Database.BeginTransaction())
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
                    
                    transaction.Commit();
                    return "success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

        }
    }
}
