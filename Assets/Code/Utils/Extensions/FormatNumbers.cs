using System.Globalization;

namespace Code.Utils.Extensions
{
    public static class FormatNumbers
    {
        public static string Cut(decimal number)
        {
            string result;

            if (number > 999999999 || number < -999999999 )
                result = number.ToString("0,,,.###B", CultureInfo.InvariantCulture);
            else if (number > 999999 || number < -999999 )
                result = number.ToString("0,,.##M", CultureInfo.InvariantCulture);
            else if (number > 999 || number < -999)
                result = number.ToString("0,.#K", CultureInfo.InvariantCulture);
            else if (number == 0)
                result = "0";
            else
                result = number.ToString("#,#", CultureInfo.InvariantCulture);

            return result;
        }
    }
}