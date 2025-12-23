namespace Nivaes.App.Cross.Droid
{

    public static class MvxJavaDateUtils
    {
        public static DateTime DateTimeFromJava(int year, int month, int dayOfMonth)
        {
            // +1 needed as Java date months are zero-based
            return new DateTime(year, month + 1, dayOfMonth, 0, 0, 0, DateTimeKind.Unspecified);
        }
    }
}