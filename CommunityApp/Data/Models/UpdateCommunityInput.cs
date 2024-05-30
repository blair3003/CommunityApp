﻿using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class UpdateCommunityInput
    {
        [Required]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
    }
}