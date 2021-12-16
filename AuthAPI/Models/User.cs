using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MoneyTrackDatabaseAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [Required, MaxLength(256)]
        public string Email { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [MaxLength(256)]
        public string Salt { get; set; }
        [MaxLength(256)]
        public string HashVersion { get; set; }
        [MaxLength(256)]
        public string ApiVersion { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<Device> Devices { get; set; }
    }
}