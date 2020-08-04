using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<ApplicationUser> lstAppUser = _context.AspNetUsers
                .Include(x=>x.GenderType)
                .ToList();
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

        public ApplicationUser GetUserAccount(string id)
        {
            throw new NotImplementedException();
        }

        public UserAccountAdministrationVwMdl UpdateAdminUserAccount(UserAccountAdministrationVwMdl userAccount)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserAccount(string id)
        {
            throw new NotImplementedException();
        }
    }
}
