using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IUserAccountService
    {
        List<UserAccountAdministrationVwMdl> GetAll();
        ApplicationUser GetUserAccount(string id);
        bool UpdateAdminUserAccount(UserAccountAdministrationVwMdl userAccount);
        void DeleteUserAccount(string id);
    }
}
