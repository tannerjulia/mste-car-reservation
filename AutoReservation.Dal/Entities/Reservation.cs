using System;
using System.ComponentModel.DataAnnotations;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        [Key]
        public int ReservationsNr { get; set; }

        public int AutoId { get; set; } //TODO fremdschlüssel

        public int KundeId { get; set; } //TODO fremdschlüssel

        [Required, MaxLength(7)]
        public DateTime Von { get; set; } //TODO datetime2(7) korrekt?

        [Required, MaxLength(7)]
        public DateTime Bis { get; set; } //TODO datetime2(7) korrekt?

        public DateTime RowVersion { get; set; } //TODO DateTime für Timestamp?
    }
}