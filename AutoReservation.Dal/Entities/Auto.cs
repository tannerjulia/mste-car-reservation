using System;    
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    // Using Data Annotations. Could also be done using the fluent API
    public abstract class Auto
    {
        [Key] 
        public int Id { get; set; }
        
        [Required, Column(TypeName = "NVARCHAR(20)")]
        public String Marke { get; set; }
        
        public int Tagestarif { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }

//        public int AutoKlasse { get; set; } chapter 3.2 AutoKlass not needed anymore
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    public class StandardAuto : Auto {
        
    }
    
    public class MittelklasseAuto : Auto {
        
    }

    public class LuxusklasseAuto : Auto {
        public int Basistarif { get; set; } //mandatory for luxury class
    }
}