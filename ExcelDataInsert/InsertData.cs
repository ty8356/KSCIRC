using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSCIRC.Models.Model;
using KSCIRC.Models.Settings;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

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

        // NOTE: tons of hard coding because this is a one-time thing
        public void Execute()
        {
            var publicationId = 1;
            // Console.WriteLine("Creating default publication...");

            // var pub = new Publication
            // {
            //     Title = "RT Contamination"
            // };

            // _context.Publications.Add(pub);
            // _context.SaveChanges();

            // publicationId = pub.Id;

            // Console.WriteLine("Default publication created.");

            Console.WriteLine("Uploading from excel file...");

            var file = new FileInfo(@"C:\kscirc\upload\Log2FC_Qvalue_Merged_ForAllResults.xlsx");

            using (var package = new ExcelPackage(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var worksheet = package?
                    .Workbook?
                    .Worksheets?
                    .FirstOrDefault();

                var totalRows = worksheet?
                    .Dimension?
                    .Rows;

                if (totalRows == null)
                {
                    Console.WriteLine("No rows found.");
                }
                else
                {
                    Console.WriteLine($"Found {totalRows} rows.");

                    var headers = worksheet.GetHeaderColumns();
                    var columns = headers.ReadColumnNumbers();

                    for (int i = 2; i <= totalRows; i++)
                    {
                        Console.WriteLine($"Adding gene {i - 1} of {totalRows}.");

                        var gene = new Gene
                        {
                            EnsId = worksheet.Cells[i, columns["ens_id"]]?.Value?.ToString() ?? "",
                            Name = worksheet.Cells[i, columns["gene"]]?.Value?.ToString() ?? "",
                            Description = worksheet.Cells[i, columns["description"]]?.Value?.ToString() ?? ""
                        };

                        _context.Genes.Add(gene);
                        _context.SaveChanges();

                        var in_2dpi = worksheet.Cells[i, columns["log2fc_in_02dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_in_02dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_in_02dpivsnaive"]]?.Value) : null;
                        var in_q_2dpi = worksheet.Cells[i, columns["q_value_in_02dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_in_02dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_in_02dpivsnaive"]]?.Value) : null;
                        var in_10dpi = worksheet.Cells[i, columns["log2fc_in_10dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_in_10dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_in_10dpivsnaive"]]?.Value) : null;
                        var in_q_10dpi = worksheet.Cells[i, columns["q_value_in_10dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_in_10dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_in_10dpivsnaive"]]?.Value) : null;
                        var in_42dpi = worksheet.Cells[i, columns["log2fc_in_42dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_in_42dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_in_42dpivsnaive"]]?.Value) : null;
                        var in_q_42dpi = worksheet.Cells[i, columns["q_value_in_42dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_in_42dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_in_42dpivsnaive"]]?.Value) : null;

                        var ip_2dpi = worksheet.Cells[i, columns["log2fc_ip_02dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ip_02dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ip_02dpivsnaive"]]?.Value) : null;
                        var ip_q_2dpi = worksheet.Cells[i, columns["q_value_ip_02dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_ip_02dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ip_02dpivsnaive"]]?.Value) : null;
                        var ip_10dpi = worksheet.Cells[i, columns["log2fc_ip_10dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ip_10dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ip_10dpivsnaive"]]?.Value) : null;
                        var ip_q_10dpi = worksheet.Cells[i, columns["q_value_ip_10dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_ip_10dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ip_10dpivsnaive"]]?.Value) : null;
                        var ip_42dpi = worksheet.Cells[i, columns["log2fc_ip_42dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ip_42dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ip_42dpivsnaive"]]?.Value) : null;
                        var ip_q_42dpi = worksheet.Cells[i, columns["q_value_ip_42dpivsnaive"]]?.Value != null && worksheet.Cells[i, columns["q_value_ip_42dpivsnaive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ip_42dpivsnaive"]]?.Value) : null;

                        var ip_vs_in_2dpi = worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value) : null;
                        var ip_vs_in_q_2dpi = worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value) : null;
                        var ip_vs_in_10dpi = worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value) : null;
                        var ip_vs_in_q_10dpi = worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value) : null;
                        var ip_vs_in_42dpi = worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value) : null;
                        var ip_vs_in_q_42dpi = worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value) : null;

                        var interaction = worksheet.Cells[i, columns["log2fc_2x4"]]?.Value != null && worksheet.Cells[i, columns["log2fc_2x4"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_2x4"]]?.Value) : null;
                        var interaction_q = worksheet.Cells[i, columns["q_value_2x4"]]?.Value != null && worksheet.Cells[i, columns["q_value_2x4"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_2x4"]]?.Value) : null;

                        var statValues = new List<StatValue>();

                        statValues.Add(new StatValue
                        {
                            GeneId = gene.Id,
                            PublicationId = publicationId,
                            DaysPostInjury = 2,
                            InputValue = in_2dpi,
                            InputQvalue = in_q_2dpi,
                            ImmunoprecipitateValue = ip_2dpi,
                            ImmunoprecipitateQvalue = ip_q_2dpi,
                            EnrichmentValue = ip_vs_in_2dpi,
                            EnrichmentQvalue = ip_vs_in_q_2dpi,
                            InteractionValue = interaction,
                            InteractionQvalue = interaction_q
                        });

                        statValues.Add(new StatValue
                        {
                            GeneId = gene.Id,
                            PublicationId = publicationId,
                            DaysPostInjury = 10,
                            InputValue = in_10dpi,
                            InputQvalue = in_q_10dpi,
                            ImmunoprecipitateValue = ip_10dpi,
                            ImmunoprecipitateQvalue = ip_q_10dpi,
                            EnrichmentValue = ip_vs_in_10dpi,
                            EnrichmentQvalue = ip_vs_in_q_10dpi,
                            InteractionValue = interaction,
                            InteractionQvalue = interaction_q
                        });

                        statValues.Add(new StatValue
                        {
                            GeneId = gene.Id,
                            PublicationId = publicationId,
                            DaysPostInjury = 42,
                            InputValue = in_42dpi,
                            InputQvalue = in_q_42dpi,
                            ImmunoprecipitateValue = ip_42dpi,
                            ImmunoprecipitateQvalue = ip_q_42dpi,
                            EnrichmentValue = ip_vs_in_42dpi,
                            EnrichmentQvalue = ip_vs_in_q_42dpi,
                            InteractionValue = interaction,
                            InteractionQvalue = interaction_q
                        });

                        _context.StatValues.AddRange(statValues);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}