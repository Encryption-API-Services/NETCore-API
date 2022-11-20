using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ThirdPartyAPIs
{
    public class IpInfoResponse
    {
        public string Ip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Loc { get; set; }
        public string Org { get; set; }
        public string Postal { get; set; }
        public string TimeZone { get; set; }
    }
}