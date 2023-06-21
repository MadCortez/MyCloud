using System.ComponentModel.DataAnnotations;

namespace MyCloud.Models
{
    public class FileData
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public double Weight { get; set; }
        public int AllMemory { get; set; } 
        public DateTime CreateDate { get; set; }   
    }
}
