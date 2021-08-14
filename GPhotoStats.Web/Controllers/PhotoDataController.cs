using System.Collections.Generic;
using System.Threading.Tasks;
using GPhotoStats.BL.Interfaces;
using GPhotoStats.BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GPhotoStats.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoDataController : ControllerBase
    {
        private readonly IGPhotosClient _gPhotosClient;
        private readonly ILogger<PhotoDataController> _logger;

        public PhotoDataController(ILogger<PhotoDataController> logger, IGPhotosClient gPhotosClient)
        {
            _logger = logger;
            _gPhotosClient = gPhotosClient;
        }

        [HttpGet]
        [Route("Sync")]
        public Task<SyncResult> Sync()
        {
            return _gPhotosClient.SyncPhotos();
        }

        [HttpGet]
        public Task<List<PhotoData>> Get()
        {
            return _gPhotosClient.RetrievePhotos();
        }
    }
}