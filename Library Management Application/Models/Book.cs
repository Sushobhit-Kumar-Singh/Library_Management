using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Application.Models;

public partial class Book
{
    [Required(ErrorMessage = "ISBN is required")]
    public string Isbn { get; set; } = null!;

    [Required]
    public string? Title { get; set; }

	[Required]
	public string? Author { get; set; }

	[Required]
	public string? Genre { get; set; }

	[Required]
	public short PublicationYear { get; set; }

	[Required]
	public int CopiesAvailable { get; set; }

	[Required]
	public int TotalCopies { get; set; }

	public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; } = true;

    [NotMapped]
    public string? SearchTerm { get; set; }

    public virtual Member? CreatedByNavigation { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Member? UpdatedByNavigation { get; set; }
}
