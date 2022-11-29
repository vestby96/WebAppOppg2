namespace WebAppOppg2
{
    public static class AppData
    {
        public static string JwtIssuer { get; set;}
        public static string JwtKey { get; set;}
    }

    public class AppNonStatic
    {
        public int MyNumber { get; set;}
    }
}
