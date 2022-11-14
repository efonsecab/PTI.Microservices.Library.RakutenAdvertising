using System;
using System.Collections.Generic;
using System.Text;

namespace PTI.Microservices.Library.Configuration
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RakutenAdvertisingConfiguration
    {
        public string BaseUrl { get; set; } = "https://api.rakutenmarketing.com";
        public string TokenRequestAuthorizationSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
