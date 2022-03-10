using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicApi.Data;
using MusicApi.Models;
using MusicApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ApiDbContext apiDbContext;

        public ArtistsController(ApiDbContext apiDbContext)
        {
            this.apiDbContext = apiDbContext;
        }

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            artist.ImageUrl = await FileHelper.UploadFile(artist.Image);
            await apiDbContext.Artists.AddAsync(artist);
            await apiDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/artists
        [HttpGet]
        public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 6;
            var artists = await (from artist in apiDbContext.Artists
                                 select new
                                 {
                                     Id = artist.Id,
                                     Name = artist.Name,
                                     ImageUrl = artist.ImageUrl,
                                 }).ToListAsync();
            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]/{artistId}")]
        public async Task<IActionResult> ArtistDetails(int artistId)
        {
           var artist = await apiDbContext.Artists.Where(a=>a.Id == artistId).Include(a => a.Songs).ToListAsync();
           return Ok(artist);
        }
    }
}
