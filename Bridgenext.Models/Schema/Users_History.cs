﻿using Bridgenext.Models.Schema.Base.Audit;
using System.ComponentModel.DataAnnotations;

namespace Bridgenext.Models.Schema
{
    public class Users_History : AuditableEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        public int IdUserType { get; set; }
    }
}
