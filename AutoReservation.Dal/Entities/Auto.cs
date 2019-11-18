using System;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Auto
    {
        [Key] 
        public int Id { get; set; }
        
        [Required, MaxLength(20)]
        public String Marke { get; set; }
        
        public int Tagestarif { get; set; }
        
        public DateTime RowVersion { get; set; } //TODO DateTime für Timestamp?

        public int AutoKlasse { get; set; }
        
        public int? Basistarif { get; set; }
    }
}