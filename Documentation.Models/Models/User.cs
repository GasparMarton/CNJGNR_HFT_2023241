using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id", TypeName = "int")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required] // nem lehet null, példányosításnál meg kell adni
        public string FirstName { get; set; }
        [Required] // nem lehet null, példányosításnál meg kell adni
        public string LastName { get; set; }
    }
}
