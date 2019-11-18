using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationsNr { get; set; }

        public int AutoId { get; set; } //TODO fremdschlüssel

        public int KundeId { get; set; } //TODO fremdschlüssel

        [Required, MaxLength(7), Column(TypeName = "DATETIME2(7)")]
        public DateTime Von { get; set; } //TODO datetime2(7) korrekt?

        [Required, MaxLength(7), Column(TypeName = "DATETIME2(7)")]
        public DateTime Bis { get; set; } //TODO datetime2(7) korrekt?

        [Column(TypeName = "TIMESTAMP")]
        public DateTime RowVersion { get; set; } //TODO DateTime für Timestamp?
        
        [ForeignKey(nameof(AutoId))]
        public Auto Auto { get; set; }

        [ForeignKey(nameof(KundeId))]
        public Kunde Kunde { get; set; }
    }
}