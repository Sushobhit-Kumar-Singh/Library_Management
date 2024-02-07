using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Application.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    [ForeignKey("Book")]
    public string? BookIsbn { get; set; }

    public int? MemberId { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Book? BookIsbnNavigation { get; set; }

    public virtual Member? CreatedByNavigation { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Member? UpdatedByNavigation { get; set; }
}
