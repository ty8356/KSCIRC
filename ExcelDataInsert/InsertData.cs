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
        private readonly hetmanlabdbContext _context;
        private readonly ApplicationSettings _applicationSettings;

        public InsertData(
            hetmanlabdbContext context
            ,IOptions<ApplicationSettings> applicationSettings
            )
        {
            _context = context;
            _applicationSettings = applicationSettings.Value;
        }

        // NOTE: tons of hard coding because this is a one-time thing
        public void Execute()
        {
            // InsertInitial();

            InsertReadCounts();
        }

        private void InsertInitial()
        {
            var publicationId = 2;
            Console.WriteLine("Creating default publication...");

            var pub = new Publication
            {
                Title = "RT Contamination"
            };

            _context.Publications.Add(pub);
            _context.SaveChanges();

            publicationId = pub.Id;

            Console.WriteLine("Default publication created.");

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

                    for (int i = 17150; i <= totalRows; i++)
                    {
                        Console.WriteLine($"Adding gene {i - 1} of {totalRows - 1}.");

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

                        var ip_vs_in_0dpi = worksheet.Cells[i, columns["log2fc_ipvsin_naive"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_naive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_naive"]]?.Value) : null;
                        var ip_vs_in_q_0dpi = worksheet.Cells[i, columns["q_value_ipvsin_naive"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_naive"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_naive"]]?.Value) : null;
                        var ip_vs_in_2dpi = worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_02dpi"]]?.Value) : null;
                        var ip_vs_in_q_2dpi = worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_02dpi"]]?.Value) : null;
                        var ip_vs_in_10dpi = worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_10dpi"]]?.Value) : null;
                        var ip_vs_in_q_10dpi = worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_10dpi"]]?.Value) : null;
                        var ip_vs_in_42dpi = worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_ipvsin_42dpi"]]?.Value) : null;
                        var ip_vs_in_q_42dpi = worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_ipvsin_42dpi"]]?.Value) : null;

                        var interaction_2dpi = worksheet.Cells[i, columns["log2fc_2x2_2dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_2x2_2dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_2x2_2dpi"]]?.Value) : null;
                        var interaction_q_2dpi = worksheet.Cells[i, columns["q_value_2x2_2dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_2x2_2dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_2x2_2dpi"]]?.Value) : null;
                        var interaction_10dpi = worksheet.Cells[i, columns["log2fc_2x2_10dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_2x2_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_2x2_10dpi"]]?.Value) : null;
                        var interaction_q_10dpi = worksheet.Cells[i, columns["q_value_2x2_10dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_2x2_10dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_2x2_10dpi"]]?.Value) : null;
                        var interaction_42dpi = worksheet.Cells[i, columns["log2fc_2x2_42dpi"]]?.Value != null && worksheet.Cells[i, columns["log2fc_2x2_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_2x2_42dpi"]]?.Value) : null;
                        var interaction_q_42dpi = worksheet.Cells[i, columns["q_value_2x2_42dpi"]]?.Value != null && worksheet.Cells[i, columns["q_value_2x2_42dpi"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_2x2_42dpi"]]?.Value) : null;

                        var interaction_2x4 = worksheet.Cells[i, columns["log2fc_2x4"]]?.Value != null && worksheet.Cells[i, columns["log2fc_2x4"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["log2fc_2x4"]]?.Value) : null;
                        var interaction_2x4_q = worksheet.Cells[i, columns["q_value_2x4"]]?.Value != null && worksheet.Cells[i, columns["q_value_2x4"]]?.Value?.ToString() != "NA" ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["q_value_2x4"]]?.Value) : null;

                        var statValues = new List<StatValue>();

                        statValues.Add(new StatValue
                        {
                            GeneId = gene.Id,
                            PublicationId = publicationId,
                            DaysPostInjury = 0,
                            EnrichmentValue = ip_vs_in_0dpi,
                            EnrichmentQvalue = ip_vs_in_q_0dpi
                        });

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
                            InteractionValue = interaction_2dpi,
                            InteractionQvalue = interaction_q_2dpi,
                            Interaction2x4Value = interaction_2x4,
                            Interaction2x4Qvalue = interaction_2x4_q
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
                            InteractionValue = interaction_10dpi,
                            InteractionQvalue = interaction_q_10dpi,
                            Interaction2x4Value = interaction_2x4,
                            Interaction2x4Qvalue = interaction_2x4_q
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
                            InteractionValue = interaction_42dpi,
                            InteractionQvalue = interaction_q_42dpi,
                            Interaction2x4Value = interaction_2x4,
                            Interaction2x4Qvalue = interaction_2x4_q
                        });

                        _context.StatValues.AddRange(statValues);
                        _context.SaveChanges();
                    }
                }
            }
        }

        private void InsertReadCounts()
        {
            Console.WriteLine("Uploading read counts from excel file...");

            var file = new FileInfo(@"C:\kscirc\upload\20220119_htseq_normalizedCounts_MinCount10_MedAveSD_02142022.xlsx");

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
                        Console.WriteLine($"Adding gene {i - 1} of {totalRows - 1}.");

                        var ensId = worksheet.Cells[i, columns["ensembl_id"]]?.Value?.ToString();
                        var inMed2dpi = worksheet.Cells[i, columns["in2dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["in2dpimed"]]?.Value?.ToString()) : null;
                        var ipMed2dpi = worksheet.Cells[i, columns["ip2dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["ip2dpimed"]]?.Value?.ToString()) : null;
                        var inMed10dpi = worksheet.Cells[i, columns["in10dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["in10dpimed"]]?.Value?.ToString()) : null;
                        var ipMed10dpi = worksheet.Cells[i, columns["ip10dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["ip10dpimed"]]?.Value?.ToString()) : null;
                        var inMed42dpi = worksheet.Cells[i, columns["in42dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["in42dpimed"]]?.Value?.ToString()) : null;
                        var ipMed42dpi = worksheet.Cells[i, columns["ip42dpimed"]]?.Value != null ? (decimal?) Convert.ToDecimal(worksheet.Cells[i, columns["ip42dpimed"]]?.Value?.ToString()) : null;

                        var gene = _context
                            .Genes
                            .FirstOrDefault(x => x.EnsId == ensId);

                        if (gene == null)
                        {
                            Console.WriteLine($"Gene {ensId ?? ""} not found. Continuing...");
                            continue;
                        }

                        var statValues = _context
                            .StatValues
                            .Where(x => x.GeneId == gene.Id)
                            .ToList();

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 2)
                            .InputMedianReadCount = inMed2dpi;

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 2)
                            .ImmunoprecipitateMedianReadCount = ipMed2dpi;

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 10)
                            .InputMedianReadCount = inMed10dpi;

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 10)
                            .ImmunoprecipitateMedianReadCount = ipMed10dpi;

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 42)
                            .InputMedianReadCount = inMed42dpi;

                        statValues
                            .FirstOrDefault(x => x.DaysPostInjury == 42)
                            .ImmunoprecipitateMedianReadCount = ipMed42dpi;

                        _context.StatValues.UpdateRange(statValues);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}