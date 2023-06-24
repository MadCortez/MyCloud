using System.ComponentModel.DataAnnotations;

namespace MyCloud.Models
{
    public class FileData
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public double Weight { get; set; }
        public long AllMemory { get; set; } 
        public DateTime CreateDate { get; set; }   
        public string Path { get; set; }   
    }
}
