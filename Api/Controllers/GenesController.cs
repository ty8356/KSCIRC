using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.ErrorHandling.Exceptions;
using KSCIRC.Models.ResponseModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace KSCIRC.Api.Controllers
{
    [ApiController]
    [Route("genes")]
    public class GenesController : ControllerBase
    {
        private readonly ILogger<GenesController> _logger;
        private readonly IMapper _mapper;
        private readonly IGeneService _geneService;

        public GenesController(ILogger<GenesController> logger
            ,IMapper mapper
            ,IGeneService geneService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _geneService = geneService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var genes = await _geneService
                .Search(name);
            
            return Ok(_mapper.Map<List<GeneResponseModel>>(genes));
        }

        [HttpGet("{name}/stat-values")]
        public async Task<IActionResult> GetStatValues(string name)
        {
            var statValues = await _geneService
                .GetStatValues(name);
            
            return Ok(_mapper.Map<List<StatValueResponseModel>>(statValues));
        }

        [HttpGet("download-all"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadSourceFile()
        {
            var filePath = Path.Combine("C:\\kscirc\\source", "Log2FC_Qvalue_Merged_ForAllResults.xlsx");
            if (!System.IO.File.Exists(filePath))
                throw new HttpNotFoundException("File not found.");

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            
            return File(memory, GetContentType(filePath), filePath);
        }

        [HttpGet("download-advanced"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadAdvanced([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var stream = new MemoryStream(await _geneService.GetExcelSheetByRange(min, max));
            stream.Position = 0;
            var date = DateTime.Now;
            
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{date.ToString("yyyy")}{date.ToString("MM")}{date.ToString("dd")}_ol_gene_exp_search.xlsx");
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
                    
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
                    
            return contentType;
        }
    }
}
