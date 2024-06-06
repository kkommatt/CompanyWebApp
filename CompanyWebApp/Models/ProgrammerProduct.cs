namespace CompanyWebApp.Models
{
    public class ProgrammerProduct
    {
        public int Id { get; set; }
        public int ProgrammerId { get; set; }
        public Programmer Programmer { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}