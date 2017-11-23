using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImVehicleCore.Data
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity and to ease caching logic
    public class BaseEntity
    {

        [Key]
        public long Id { get; set; }

        public int VersionNumber { get; set; }
        public string Metadata { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        
        public long ModifyBy { get; set; }
        public int Status { get; set; }
    }
}
