using System;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Kunde
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MaxLength(20)]
        public String Nachname { get; set; }

        [Required, MaxLength(20)]
        public String Vorname { get; set; }

        [Required, MaxLength(7)]
        public DateTime Geburtsdatum { get; set; }
        
        public DateTime RowVersion { get; set; } //TODO DateTime für Timestamp?
    }
}