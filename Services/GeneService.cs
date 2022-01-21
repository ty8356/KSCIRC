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
        private readonly hetmanlabdbContext _context;

        public GeneService(
            hetmanlabdbContext context
        )
        {
            _context = context;
        }

        public async Task<List<Gene>> Search(string name)
        {
            var genes = _context
                .Genes
                .Include(x => x.StatValues)
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
                        .StartsWith(name));
            }

            return await genes
                .ToListAsync();
        }

        public async Task<List<StatValue>> GetStatValues(string name)
        {
            var gene = await _context
                .Genes
                .Include(x => x.StatValues)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();

            return gene
                .StatValues
                .ToList();
        }
    }
}
