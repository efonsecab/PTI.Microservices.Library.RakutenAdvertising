using System;
using System.Collections.Generic;
using System.Text;

namespace PTI.Microservices.Library.Models.RakutenAdvertisingService.GetBearerToken
{

    /// <summary>
    /// 
    /// </summary>
    public class GetBearerTokenResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
    }

}
