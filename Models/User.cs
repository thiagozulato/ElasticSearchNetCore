using System;
using System.ComponentModel.DataAnnotations;

namespace ElasticSearchDotNet
{
    public class User
    {
        [Required(ErrorMessage = "Invalid UserID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User birthdate is required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "User document is required")]
        public string Document { get; set; }

        [Required(ErrorMessage = "User country is required")]
        public string Country { get; set; }

        public void CreateId()
        {
            Id = Guid.NewGuid();
        }
    }
}