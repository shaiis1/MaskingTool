using maskingTool;
using maskingTool.Interfaces;
using maskingTool.Utils;
using MaskIPWebAPI.BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MaskIPWebAPI.Controllers
{
    [Route("MaskIP")]
    [ApiController]
    public class MaskIPController : ControllerBase
    {
        private readonly IMask _iMask;
        private readonly ILogger<MaskIPController> _logger;

        public MaskIPController(ILogger<MaskIPController> logger, IMask iMask)
        {
            _logger = logger;
            _iMask = iMask;
        }

        [Route("MaskIP")]
        [HttpPost]
        public async Task<IActionResult> MaskIP()
        {
            try
            {
                _logger.LogDebug($"***************** Start MaskIP ****************");
                string filecontents;
                var files = Request.Form.Files;
                using(var stream = files[0].OpenReadStream())
                using(var reader = new StreamReader(stream))
                {
                    filecontents = await reader.ReadToEndAsync();
                }
                var readedText = FileReaderBL.GetStringWithoutApostrophes(filecontents);
                var maskedIP = _iMask.MaskAllIPs(readedText, Regex.IPV4_RGX);
                byte[] byteArray = Encoding.UTF8.GetBytes(maskedIP);
                MemoryStream memorystream = new MemoryStream(byteArray);
                _logger.LogDebug($"***************** Done MaskIP ****************");
                return new FileStreamResult(memorystream, "text/plain")
                {
                    FileDownloadName = "Maskedfile.log"
                };
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"MaskIP ERROR: {ex.Message}");
                //return BadRequest(ex.Message);
                return new FileStreamResult(new MemoryStream(), "text/plain");
            }
        }
    }
}
