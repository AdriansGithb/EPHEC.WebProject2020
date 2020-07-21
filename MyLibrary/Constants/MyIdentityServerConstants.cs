namespace MyLibrary.Constants
{
    public static class MyIdentityServerConstants
    {
        public const string IS_Url = "https://localhost:44386/";

        public const string ConnectionString = @"Data Source=MSI\SQLEXPRESS;database=WebProject2020;trusted_connection=yes;";

        public const string Role_User = "user";
        public const string Role_Manager = "manager";
        public const string Role_Admin = "admin";
        public const string Role_ManagerOrUser = "manager, user";        
        public const string Role_AdminOrManager = "admin, manager";
        public const string Role_AdminOrManagerOrUser = "admin, manager, user";

    }
}
