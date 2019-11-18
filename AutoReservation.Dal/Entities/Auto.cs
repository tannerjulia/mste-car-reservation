using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Auto
    {
        [Key] 
        public int Id { get; set; }
        
        [Required, MaxLength(20), Column(TypeName = "NVARCHAR(20)")]
        public String Marke { get; set; }
        
        public int Tagestarif { get; set; }
        
        [Column(TypeName = "TIMESTAMP")]
        public DateTime RowVersion { get; set; } //TODO DateTime für Timestamp?

        public int AutoKlasse { get; set; }
        
        public int? Basistarif { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}