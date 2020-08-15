using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLibrary.Entities;

namespace MyLibrary.DTOs
{
    public class NewsDTO
    {
        [DisplayName("# News")]
        public string Id { get; set; }

        [DisplayName("# Establishment")]
        public int EstablishmentId { get; set; }
        [DisplayName("Establishment")]
        public string EstablishmentName { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Last Updated")]
        public DateTime UpdatedDate { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("Title")]
        public string Title { get; set; }
    }
}
