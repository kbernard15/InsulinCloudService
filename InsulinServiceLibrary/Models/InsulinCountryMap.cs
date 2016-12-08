using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{
    [DataContract]
    public class InsulinCountryMap
    {
        [DataMember]
        public Guid InsulinGUID { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
    }
}