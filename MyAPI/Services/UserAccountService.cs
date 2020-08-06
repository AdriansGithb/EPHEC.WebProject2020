using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly AppDbContext _context;

        public UserAccountService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserAccountAdministrationVwMdl> GetAll()
        {
            List<ApplicationUser> lstAppUser = _context.AspNetUsers.ToList();
            List<UserAccountAdministrationVwMdl> rtrnList = new List<UserAccountAdministrationVwMdl>();
            foreach (ApplicationUser user in lstAppUser)
            {
                UserAccountAdministrationVwMdl userAccount = new UserAccountAdministrationVwMdl
                {
                    Email = user.Email,
                    Id = user.Id,
                    IsAdmin = user.IsAdmin,
                    IsProfessional = user.IsProfessional,
                    Username = user.UserName
                };
                rtrnList.Add(userAccount);
            }

            return rtrnList;

        }

        public UserAccountVwMdl GetUserAccount(string id)
        {
            try
            {
                ApplicationUser user = _context.AspNetUsers.First(x => x.Id == id);
                GenderTypes gender = _context.Gender_Types.First(y => y.Id == user.GenderType_Id);
                UserAccountVwMdl userAccount = new UserAccountVwMdl
                {
                    Id = user.Id,
                    Username = user.UserName,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    BirthDate = user.BirthDate,
                    IsProfessional = user.IsProfessional,
                    IsAdmin = user.IsAdmin,
                    Gender = gender.Name
                };

                return userAccount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DeleteUserAccount(string id)
        {
            try
            {
                _context.AspNetUsers.Remove(_context.AspNetUsers.Find(id));
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
