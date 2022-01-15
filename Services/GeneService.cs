using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Gene>> Search(string name)
        {
            var genes = _context
                .Genes
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                name = name
                    .ToUpper()
                    .Trim();

                genes = genes
                    .Where(x => x
                        .Name
                        .ToUpper()
                        .Contains(name));
            }

            return await genes
                .ToListAsync();
        }
    }
}
