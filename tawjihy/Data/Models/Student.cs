using System.ComponentModel.DataAnnotations;

namespace tawjihy.Data.Models
{
    public class Student
    {
        public int Id { get; set; }    
        [Required]
        public int SeatingNo { get; set; }
        [Required]
        public int IdNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Branch { get; set; }
        public string Gender { get; set; }
        [Required]
        public double Rate { get; set; }
        public string? SchoolName { get; set; }
        public string? DirectorateName { get; set; }
        public string? PhoneNumber { get; set; }


    }

}
