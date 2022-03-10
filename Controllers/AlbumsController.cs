using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicApi.Models;
using MusicApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly ApiDbContext apiDbContext;

        public AlbumsController(ApiDbContext apiDbContext)
        {
            this.apiDbContext = apiDbContext;
        }

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {
            album.ImageUrl = await FileHelper.UploadFile(album.Image);
            await apiDbContext.Albums.AddAsync(album);
            await apiDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 6;
            var albums = await (from album in apiDbContext.Albums
                               select new
                               {
                                   Id = album.Id,
                                   Name = album.Name,
                                   ImageUrl = album.ImageUrl,
                               }).ToListAsync();
            return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]/{albumId}")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var album = await apiDbContext.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
            return Ok(album);
        }
    }
}
