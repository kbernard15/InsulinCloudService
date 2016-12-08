using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{
    [DataContract]
    public class InsulinNameTranslation
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public Guid GUID { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }
}