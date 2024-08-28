using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Shared
{
    class GeoPosition
    {
        [DataContract]
        internal class jsonItem
        {
            [DataMember]
            internal string ip { get; set; }
            [DataMember]
            internal string country { get; set; }
            [DataMember]
            internal string countryCode { get; set; }
            [DataMember]
            internal string regionName { get; set; }
            [DataMember]
            internal string city { get; set; }
            [DataMember]
            internal string zip { get; set; }
            [DataMember]
            internal string time_zone { get; set; }
            [DataMember]
            internal double lat { get; set; }
            [DataMember]
            internal double lon { get; set; }
            [DataMember]
            internal int metro_code { get; set; }


        }
    }
}
