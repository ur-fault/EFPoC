using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFPoC.DAL.Models;
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int Likes { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
