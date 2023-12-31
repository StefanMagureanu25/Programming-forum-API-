﻿using System.ComponentModel.DataAnnotations;

namespace MagureanuStefan_API.Models.Authentication
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
