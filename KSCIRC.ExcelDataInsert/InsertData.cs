using System;
using KSCIRC.Models.Model;
using KSCIRC.Models.Settings;
using Microsoft.Extensions.Options;

namespace KSCIRC.ExcelDataInsert
{
    public class InsertData
    {
        private readonly KSCIRC_devContext _context;
        private readonly ApplicationSettings _applicationSettings;

        public InsertData(
            KSCIRC_devContext context
            ,IOptions<ApplicationSettings> applicationSettings
            )
        {
            _context = context;
            _applicationSettings = applicationSettings.Value;
        }

        public void Execute()
        {
            Console.WriteLine("Retrieving unprocessed fyxvenpay records...");

            
        }
    }
}