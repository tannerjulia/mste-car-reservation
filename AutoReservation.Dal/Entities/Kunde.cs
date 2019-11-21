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
        
        [Required, Column(TypeName = "NVARCHAR(20)")]
        public String Nachname { get; set; }

        [Required, Column(TypeName = "NVARCHAR(20)")]
        public String Vorname { get; set; }

        [Required, Column(TypeName = "DATETIME2(0)")]
        public DateTime Geburtsdatum { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}