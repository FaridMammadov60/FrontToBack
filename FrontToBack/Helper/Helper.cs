namespace FrontToBack.Helper
{
    public static class Helper
    {
        public static void DeleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
    public enum UserRoles
    {
        Member = 1,
        Manager = 2,
        Admin = 4,
        SuperAdmin = 8,
    }
}
