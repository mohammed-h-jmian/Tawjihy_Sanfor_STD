using Microsoft.Build.Framework;

namespace tawjihy.Data.Models
{
    public class University
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string URL { get; set; }
    }
}
