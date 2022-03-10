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
    public class SongsController : ControllerBase
    {
        private readonly ApiDbContext apiDbContext;

        public SongsController(ApiDbContext apiDbContext)
        {
            this.apiDbContext = apiDbContext;
        }

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            song.ImageUrl = await FileHelper.UploadFile(song.Image);
            song.AudioUrl = await FileHelper.UploadAudio(song.Audio);
            song.UploadedDate = DateTime.Now;
            await apiDbContext.Songs.AddAsync(song);
            await apiDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 6;
            var songs = await (from song in apiDbContext.Songs
                        select new
                        {
                            Id = song.Id,
                            Title = song.Title,
                            Duration = song.Duration,
                            ImageUrl = song.ImageUrl,
                            AudioUrl = song.AudioUrl,
                        }).ToListAsync();
            return Ok(songs.Skip((currentPageNumber-1)*currentPageSize).Take(currentPageSize));
        }
         
        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in apiDbContext.Songs
                               where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).ToListAsync();
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in apiDbContext.Songs
                               orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).Take(10).ToListAsync();
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in apiDbContext.Songs
                               where song.Title.ToLower().Contains(query.ToLower())
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).Take(10).ToListAsync();
            return Ok(songs);
        }

    }
}
