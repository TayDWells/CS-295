namespace BarberShop.Models
{
    public class Appointment
    {
        public int ApptId { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; } 

        // Navigation properties
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
    }
}
