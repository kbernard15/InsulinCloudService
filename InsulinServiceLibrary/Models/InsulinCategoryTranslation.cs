using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{
    [DataContract]
    public class InsulinCategoryTranslation
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string TypeKey { get; set; }
        [DataMember]
        public string Translation { get; set; }
    }
}