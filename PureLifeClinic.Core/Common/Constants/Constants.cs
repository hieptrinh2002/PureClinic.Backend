namespace PureLifeClinic.Core.Common.Constants
{
    public static class Constants
    {
        public const int MaxDoctorAppointmentPerday = 10;
        public const int MaxWorkingHourPerDayOfDoctor = 9; // hours
        public static readonly int AvgAppointmentTimeInMinute = 30; // phút
        public static readonly int MaxDoctorAppointmentPerDay = 15; // phút
        public static readonly int MaxDoctorWorkingHoursPerDay = 8; // phút
    }

    public static class ErrorCode
    {
        public static string InputValidateError = "INPUT_VALIDATION_ERROR";
        public static string DuplicateNameError = "DUPLICATE_NAME";
        public static string DuplicateEmailError = "DUPLICATE_EMAIL";
        public static string DuplicateUserNameError = "DUPLICATE_USERNAME";
        public static string DuplicateCodeError = "DUPLICATE_CODE";
    }
}
