using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Movies
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Title can not be empty!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Year can not be empty!")]
        [Range(868, 2025, ErrorMessage = "Invalid Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Director can not be empty!")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Director must contain only letters.")]
        public string? Director { get; set; }

        [Required(ErrorMessage = "Genre can not be empty!")]
        public string? Genre { get; set; }


        public string? Img { get; set; }

        [StringLength(512, MinimumLength = 1, ErrorMessage = "Info length must be between 1 and 512 characters.")]
        public string? Info { get; set; }


    }
}
