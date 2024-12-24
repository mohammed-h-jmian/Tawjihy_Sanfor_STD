using System.ComponentModel.DataAnnotations;

namespace tawjihy.Data.Models
{
    public class Visitor
    {
        public int Id { get; set; }
        [Required]
        public int SeatingNo { get; set; }
    }
}
