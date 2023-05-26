using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contracts.Account
{
    public class RegisterAccount
    {
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Fullname { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Username { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Password { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Mobile { get; set; }

        public long RoleId { get; set; }

        public IFormFile ProfilePhoto { get; set; }
        
    }
}
