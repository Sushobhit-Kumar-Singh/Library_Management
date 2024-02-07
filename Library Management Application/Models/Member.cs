using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Library_Management_Application.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? RoleType { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Book> BookCreatedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<Book> BookUpdatedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionMembers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionUpdatedByNavigations { get; set; } = new List<Transaction>();
}
