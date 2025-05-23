﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task1LoginRegister.Models;

public partial class Userlogin
{
    public int Id { get; set; }
    [Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = null!;
    [Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = null!;
    [Required]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;
    [Phone]
    [Required]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string Phone { get; set; } = null!;
    [Required]
    [DisplayName("Profile Picture")]
    public string Photo { get; set; } = null!;
    [Required]
    public string Gender { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; }
    public virtual ICollection<Cart> Carts { get; set; }

    public virtual ICollection<Order> Orders { get; set; } // Relationship with Order

    public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; } // Relationship with DeliveryAddress
    public virtual ICollection<Review> Reviews { get; set; }
}
