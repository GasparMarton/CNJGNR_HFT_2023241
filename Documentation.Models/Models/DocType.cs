using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.Models
{
    [Table("doctype")]
    public class DocType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("doctype_id", TypeName = "int")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required] // nem lehet null, példányosításnál meg kell adni
        public string Name { get; set; }
        public bool FinalState { get; set; }

        [NotMapped]
        public virtual Document[] Documents { get; set; }
        [NotMapped]
        public virtual int TotalDocuments { get; set; }

    }
}
