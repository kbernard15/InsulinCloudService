using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace InsulinServiceLibrary.Models
{
    [DataContract]
    public class Insulin
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid GUID { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string CategoryTypeKey { get; set; }
    }
}