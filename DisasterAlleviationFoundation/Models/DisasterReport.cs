using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class DisasterReport
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Severity { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateReported { get; set; }
    }
}