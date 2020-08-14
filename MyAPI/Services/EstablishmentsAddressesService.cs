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
                    .Where(x => x.EstablishmentId == 5||x.EstablishmentId==4||x.EstablishmentId==6)
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
                        BoxNumber = dbAddress.BoxNumber
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
            throw new NotImplementedException();
        }
    }
}
