using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.ErrorHandling.Exceptions;
using KSCIRC.Models.ResponseModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KSCIRC.Api.Controllers
{
    [ApiController]
    [Route("api/genes")]
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
    }
}
