using System.Collections.Generic;
using System.Threading.Tasks;
using KSCIRC.Models.Model;

namespace KSCIRC.Interfaces.Services
{
    public interface IGeneService
    {
        public Task<List<Gene>> Search(string name);
    }
}