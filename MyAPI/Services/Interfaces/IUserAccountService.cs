using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IUserAccountService
    {
        List<UserAccountAdministrationVwMdl> GetAll();
        ApplicationUser GetUserAccount(string id);
        UserAccountAdministrationVwMdl UpdateAdminUserAccount(UserAccountAdministrationVwMdl userAccount);
        void DeleteUserAccount(string id);
    }
}
