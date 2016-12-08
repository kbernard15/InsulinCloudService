using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace InsulinServiceLibrary.Models
{

    public class parameters
    {
        public string name;
        public string description;
    }

    public class apiRef
    {
        public string name;
        public string url;
        public string description;

        public IList<parameters> parameters;

    }

    [DataContract]
    public class ApiReference
    {
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public IList<apiRef> api { get; set; }
    }
}