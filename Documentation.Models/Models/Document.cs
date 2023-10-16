using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.Models
{
    [Table("document")]
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doc_id", TypeName = "int")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required] // nem lehet null, példányosításnál meg kell adni
        public User Employee { get; set; }
        public string Comment { get; set; }
        [MaxLength(255)]
        public string Link { get; set; }
        public DocStatus Status { get; set; }

        [ForeignKey(nameof(DocStatus))] // meg kell adni, melyik navigation propertyre vonatkozik (Brand)
        public int DocStatus_Id { get; set; }
        public DocType Type { get; set; }

        [ForeignKey(nameof(DocStatus))] // meg kell adni, melyik navigation propertyre vonatkozik (Brand)
        public int DocType_Id { get; set; }
    }
}
