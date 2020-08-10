using System.Runtime.CompilerServices;

namespace MyLibrary.Constants
{
    public static class MyIdentityServerConstants
    {
        public const string IS_Url = "https://localhost:44386/";
        public const string ISRegister_Url = "https://localhost:44386/Account/Register";
        public const string ISUserEditing_Url = "https://localhost:44386/Account/EditUserDetails";


        public const string Role_User = "user";
        public const string Role_Manager = "manager";
        public const string Role_Admin = "admin";
        public const string Role_Admin_Manager = "admin, manager";


    }
}
