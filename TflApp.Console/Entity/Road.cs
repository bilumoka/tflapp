using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TflApp.Console.Entity
{
    [DataContract(Name = "road")]
    public class Road
    {
        [DataMember(Name = "$type")]
        public string Type { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "statusSeverity")]
        public string StatusSeverity { get; set; }

        [DataMember(Name = "statusSeverityDescription")]
        public string StatusSeverityDescription { get; set; }

        [DataMember(Name = "bounds")]
        public string Bounds { get; set; }

        [DataMember(Name = "envelope")]
        public string Envelope { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
