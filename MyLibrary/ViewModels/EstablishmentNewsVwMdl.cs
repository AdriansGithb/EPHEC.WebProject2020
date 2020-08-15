using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.ViewModels
{
    public class EstablishmentNewsVwMdl
    {
        public string Id { get; set; }

        public int EstablishmentId { get; set; }
        public string EstablishmentName { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true ,DataFormatString = "{0:U}")]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:U}")]
        public DateTime UpdatedDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

    }
}
