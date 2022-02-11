using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KSCIRC.Models.Model;

namespace KSCIRC.Interfaces.Services
{
    public interface IGeneService
    {
        public Task<List<Gene>> Search(string name);
        public Task<List<StatValue>> GetStatValues(string name);
        public Task<byte[]> GetExcelSheetByRange(decimal min, decimal max);
    }
}