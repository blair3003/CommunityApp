﻿using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Pages.Communities.InputModels
{
    public class CreateCommunityInput
    {
        [Required]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
    }
}
