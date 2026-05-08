using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookStore.Domain.Entities;

namespace bookStore.Domain.Entities.Favorite
{
    public class Favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(BookId))]
        public int BookId { get; set; }

        public UserData User { get; set; } = null!;
        public virtual Book Book { get; set; }  
    }
}
