using System;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.Model;

namespace KSCIRC.Services
{
    public class GeneService : IGeneService
    {
        private readonly KSCIRC_devContext _context;

        public GeneService(
            KSCIRC_devContext context
        )
        {
            _context = context;
        }
    }
}
