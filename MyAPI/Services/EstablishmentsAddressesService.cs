using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;

namespace MyAPI.Services
{
    public class EstablishmentsAddressesService : IEstablishmentsAddressesService
    {
        private readonly AppDbContext _context;
        public EstablishmentsAddressesService(AppDbContext context)
        {
            _context = context;
        }

        public List<AddressDTO> GetAll()
        {
            try
            {
                var allAddressesWithEstab = _context.EstablishmentsAddresses
                    .Include(x => x.Establishment)
                    .ThenInclude(y=>y.Type)
                    .ToList();

                List<AddressDTO> fullList = new List<AddressDTO>();
                foreach (var dbAddress in allAddressesWithEstab)
                {
                    AddressDTO address = new AddressDTO
                    {
                        EstablishmentId = dbAddress.EstablishmentId,
                        EstablishmentName = dbAddress.Establishment.Name,                        
                        EstablishmentType = dbAddress.Establishment.Type.Name,
                        Country = dbAddress.Country,
                        City = dbAddress.City,
                        ZipCode = dbAddress.ZipCode,
                        Street = dbAddress.Street,
                        HouseNumber = dbAddress.HouseNumber,
                        BoxNumber = dbAddress.BoxNumber,
                        OpenHour = "--",
                        CloseHour = "--"
                    };
                    if (address.BoxNumber == null)
                        address.BoxNumber = "";
                    fullList.Add(address);
                }

                return fullList;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<AddressDTO> GetAllOpen()
        {
            try
            {
                var allEstabWithScheduleWithAddress = _context.Establishments
                    .Include(x => x.Address)
                    .Include(y => y.Type)
                    .Include(z=>z.OpeningTimes)
                    .ToList();


                List<AddressDTO> openList = new List<AddressDTO>();

                DayOfWeek today = DateTime.Today.DayOfWeek;
                DayOfWeek yesterday = DateTime.Today.AddDays(-1.0).DayOfWeek;
                foreach (var estab in allEstabWithScheduleWithAddress)
                {

                    bool addToList  = false;
                    DateTime open = new DateTime();
                    DateTime close = new DateTime();
                    if (estab.OpeningTimes.First(d => d.DayOfWeek == today).IsOpen)
                    {
                        open = (DateTime)estab.OpeningTimes.Find(d => d.DayOfWeek == today).OpeningHour;
                        close = (DateTime)estab.OpeningTimes.Find(d => d.DayOfWeek == today).ClosingHour;

                        //si heure de fermeture est plus petite, l'établissement ferme le lendemain matin 
                        if (close.TimeOfDay < open.TimeOfDay)
                        {
                            close = DateTime.MaxValue;
                        }

                        //verif de l'heure d'ouverture
                        if (open.TimeOfDay < DateTime.Now.TimeOfDay && close.TimeOfDay > DateTime.Now.TimeOfDay)
                            addToList = true;

                        close = (DateTime)estab.OpeningTimes.Find(d => d.DayOfWeek == today).ClosingHour;

                    }
                    else if (!addToList 
                             && estab.OpeningTimes.First(d => d.DayOfWeek == yesterday).IsOpen)
                    {
                        open = (DateTime) estab.OpeningTimes.Find(d => d.DayOfWeek == yesterday).OpeningHour;
                        close = (DateTime)estab.OpeningTimes.Find(d => d.DayOfWeek == yesterday).ClosingHour;
                        
                        //si heure de fermeture est plus petite, l'établissement était ouvert d'hier à aujourd'hui 
                        if (close.TimeOfDay < open.TimeOfDay)
                        {
                            open = DateTime.MinValue; 
                            
                            //verif de l'heure d'ouverture
                            if (open.TimeOfDay < DateTime.Now.TimeOfDay && close.TimeOfDay > DateTime.Now.TimeOfDay)
                                addToList = true;
                        }
                        
                        open = (DateTime)estab.OpeningTimes.Find(d => d.DayOfWeek == yesterday).OpeningHour;

                    }

                    if (addToList)
                    {
                        AddressDTO address = new AddressDTO
                        {
                            EstablishmentId = estab.Id,
                            EstablishmentName = estab.Name,
                            EstablishmentType = estab.Type.Name,
                            Country = estab.Address.Country,
                            City = estab.Address.City,
                            ZipCode = estab.Address.ZipCode,
                            Street = estab.Address.Street,
                            HouseNumber = estab.Address.HouseNumber,
                            BoxNumber = estab.Address.BoxNumber,
                            OpenHour = open.ToShortTimeString(),
                            CloseHour = close.ToShortTimeString()
                        };
                        if (address.BoxNumber == null)
                            address.BoxNumber = "";
                        openList.Add(address);
                    }
                }
                
                return openList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
