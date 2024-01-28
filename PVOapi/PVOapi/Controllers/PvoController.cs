using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PVOapi.Services;
using System;
using System.IO;

namespace PVOapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PvoController : ControllerBase
    {
        private readonly IPvoService _pvoService;

        public PvoController(IPvoService pvoService)
        {
            _pvoService = pvoService ?? throw new ArgumentNullException(nameof(pvoService));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0]; 
                var result = await _pvoService.UploadFile(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}