using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(20), Column(TypeName = "NVARCHAR(20)")]
        public String Nachname { get; set; }

        [Required, MaxLength(20), Column(TypeName = "NVARCHAR(20)")]
        public String Vorname { get; set; }

        [Required, MaxLength(7), Column(TypeName = "DATETIME2(7)")]
        public DateTime Geburtsdatum { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}