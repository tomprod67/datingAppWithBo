using System;

namespace Generic.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)           // this permet de faire une Extensions sur toutes les variables qui comporte le type DateTime
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}