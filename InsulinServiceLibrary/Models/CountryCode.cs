using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{
    public class CountryCode
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}