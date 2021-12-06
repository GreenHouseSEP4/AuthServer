using System;
using JWT;

namespace MoneyTrackDatabaseAPI.Models
{
    public class ApiError
    {
        public string Title { get; set; }
        public string TimeStamp { get; set; }

        public ApiError(string title)
        {
            Title = title;
            TimeStamp = new UtcDateTimeProvider().GetNow().ToString();
        }
    }
}