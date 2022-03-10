using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicApi.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool IsFeatured { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile Audio { get; set; }

        public string AudioUrl { get; set; }

        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }

    }
}
