using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.ErrorHandling.Exceptions;
using KSCIRC.Models.ExcelModel;
using KSCIRC.Models.Model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

        public async Task<byte[]> GetExcelSheetByRange(decimal min, decimal max)
        {
            var dpis = new List<int> { 0, 2, 10, 42 };

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excel = new ExcelPackage();

            // total mRNA
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi &&
                        x.InputValue >= min &&
                        x.InputValue <= max &&
                        x.InputQvalue <= 0.05m)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InputValue,
                        QValue = x.InputQvalue
                    })
                    .OrderByDescending(x => x.Value)
                    .ToList();

                var workSheet = excel.Workbook.Worksheets.Add($"Total mRNA {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // OL mRNA
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi &&
                        x.ImmunoprecipitateValue >= min &&
                        x.ImmunoprecipitateValue <= max &&
                        x.ImmunoprecipitateQvalue <= 0.05m)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.ImmunoprecipitateValue,
                        QValue = x.ImmunoprecipitateQvalue
                    })
                    .OrderByDescending(x => x.Value)
                    .ToList();

                var workSheet = excel.Workbook.Worksheets.Add($"OL mRNA {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // OL Enrichment
            foreach(var dpi in dpis)
            {
                var genes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi &&
                        x.EnrichmentValue >= min &&
                        x.EnrichmentValue <= max &&
                        x.EnrichmentQvalue <= 0.05m)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.EnrichmentValue,
                        QValue = x.EnrichmentQvalue
                    })
                    .OrderByDescending(x => x.Value)
                    .ToList();

                var workSheet = excel.Workbook.Worksheets.Add($"OL Enrichment {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // Change in OL Enrichment
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi &&
                        x.InteractionValue >= min &&
                        x.InteractionValue <= max &&
                        x.InteractionQvalue <= 0.05m)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InteractionValue,
                        QValue = x.InteractionQvalue
                    })
                    .OrderByDescending(x => x.Value)
                    .ToList();

                var workSheet = excel.Workbook.Worksheets.Add($"Change in OL Enrichment {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            var byteArray = await excel.GetAsByteArrayAsync();

            excel.Dispose();

            return byteArray;
        }

        public async Task<byte[]> GetExcelSheetByTop(int num, string reg)
        {
            var dpis = new List<int> { 0, 2, 10, 42 };

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excel = new ExcelPackage();

            // total mRNA
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = new List<ExcelGeneModel>();

                var tempGenes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InputValue,
                        QValue = x.InputQvalue
                    });

                if (reg == "up")
                {
                    genes = tempGenes
                        .OrderByDescending(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else if (reg == "down")
                {
                    genes = tempGenes
                        .OrderBy(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else
                {
                    throw new HttpBadRequestException("reg value must be 'up' or 'down'");
                }

                var workSheet = excel.Workbook.Worksheets.Add($"Total mRNA {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // OL mRNA
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = new List<ExcelGeneModel>();

                var tempGenes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InputValue,
                        QValue = x.InputQvalue
                    });

                if (reg == "up")
                {
                    genes = tempGenes
                        .OrderByDescending(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else if (reg == "down")
                {
                    genes = tempGenes
                        .OrderBy(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else
                {
                    throw new HttpBadRequestException("reg value must be 'up' or 'down'");
                }

                var workSheet = excel.Workbook.Worksheets.Add($"OL mRNA {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // OL Enrichment
            foreach(var dpi in dpis)
            {
                var genes = new List<ExcelGeneModel>();

                var tempGenes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InputValue,
                        QValue = x.InputQvalue
                    });

                if (reg == "up")
                {
                    genes = tempGenes
                        .OrderByDescending(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else if (reg == "down")
                {
                    genes = tempGenes
                        .OrderBy(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else
                {
                    throw new HttpBadRequestException("reg value must be 'up' or 'down'");
                }

                var workSheet = excel.Workbook.Worksheets.Add($"OL Enrichment {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            // Change in OL Enrichment
            foreach(var dpi in dpis)
            {
                if (dpi == 0) continue;

                var genes = new List<ExcelGeneModel>();

                var tempGenes = _context
                    .StatValues
                    .Include(x => x.Gene)
                    .Where(x => x.DaysPostInjury == dpi)
                    .Select(x => new ExcelGeneModel 
                    {
                        Ens_Id = x.Gene.EnsId,
                        Name = x.Gene.Name,
                        Value = x.InputValue,
                        QValue = x.InputQvalue
                    });

                if (reg == "up")
                {
                    genes = tempGenes
                        .OrderByDescending(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else if (reg == "down")
                {
                    genes = tempGenes
                        .OrderBy(x => x.Value)
                        .Take(num)
                        .ToList();
                }
                else
                {
                    throw new HttpBadRequestException("reg value must be 'up' or 'down'");
                }

                var workSheet = excel.Workbook.Worksheets.Add($"Change in OL Enrichment {dpi} DPI");
            
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
        
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
            
                workSheet.Cells[1, 1].Value = "Ens_Id";
                workSheet.Cells[1, 2].Value = "Name";
                workSheet.Cells[1, 3].Value = "Value";
                workSheet.Cells[1, 4].Value = "Q Value";
        
                int recordIndex = 2;

                foreach (var gene in genes)
                {
                    workSheet.Cells[recordIndex, 1].Value = gene.Ens_Id;
                    workSheet.Cells[recordIndex, 2].Value = gene.Name;
                    workSheet.Cells[recordIndex, 3].Value = gene.Value;
                    workSheet.Cells[recordIndex, 4].Value = gene.QValue;
                    recordIndex++;
                }

                // workSheet.Column(1).AutoFit();
                // workSheet.Column(2).AutoFit();
                // workSheet.Column(3).AutoFit();
                // workSheet.Column(4).AutoFit();
            }

            var byteArray = await excel.GetAsByteArrayAsync();

            excel.Dispose();

            return byteArray;
        }
    }
}
