﻿namespace HowTo.WebApi.Models
{
    public class TokenModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string scope { get; set; }
    }
}
