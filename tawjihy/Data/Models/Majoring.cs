using Microsoft.Build.Framework;

namespace tawjihy.Data.Models
{
    public class Majoring
    {
        public int Id { get; set; }
        [Required]
        public string University { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Gender { get; set; }
        [Required]
        public string College { get; set; }
        [Required]
        public string Degree { get; set; }
        public double? Scientific { get; set; }
        public double? Literary { get; set; }
        public double? Industrial { get; set; }
        public double? Agrarian { get; set; }
        public double? Entrepreneurship { get; set; }
        public double? Lawful { get; set; }



    }
}
