using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Task1LoginRegister.DTOs;
using Task1LoginRegister.Interfaces;

namespace Task1LoginRegister.Services
{
    public class PdfReportService
    {
        private readonly IWebHostEnvironment environment;

        public PdfReportService(IWebHostEnvironment environment)
        {
            this.environment = environment;
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        public byte[] GenerateReport<T>(ReportConfig<T> reportConfig) where T : class
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    // Header
                    page.Header().Element(headerContainer =>
                    {
                        ComposeReportHeader(headerContainer, reportConfig);
                    });

                    //Content
                    page.Content().Element(container =>
                       ComposeReportContent(container, reportConfig));

                    //Footer with page numbers
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber().FontSize(8);
                        x.Span(" / ").FontSize(8);
                        x.TotalPages().FontSize(8);
                    });
                });
            }).GeneratePdf();
        }

        // report header
        private void ComposeReportHeader(IContainer headerContainer, IReportConfig reportConfig)
        {
            // logo path
            var logoPath = Path.Combine(environment.WebRootPath, "Images", "logo.png");

            headerContainer.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeColumn(3).Stack(stack =>
                    {
                        // logo and company name
                        stack.Item().Row(innerRow =>
                        {
                            if (File.Exists(logoPath))
                            {
                                innerRow.ConstantColumn(50).Image(logoPath);
                            }
                            innerRow.RelativeColumn().PaddingLeft(10)
                            .Text("Sky Mart", TextStyle.Default.FontSize(16).SemiBold());
                        });
                        //Tagline and contact info
                        stack.Item().PaddingTop(5)
                                .Text("Your Trusted Online Marketplace", TextStyle.Default.FontSize(10));

                        stack.Item().PaddingBottom(5).Text(text =>
                        {
                            text.Span("Email: ").SemiBold().FontSize(10);
                            text.Span("support@skymart.com | ").FontSize(10);
                            text.Span("Contact: ").SemiBold().FontSize(10);
                            text.Span("+91 98765 43210").FontSize(10);
                        });
                    });

                    row.RelativeColumn(2).AlignRight().Stack(stack =>
                    {
                        //dynamic report title and details
                        stack.Item().Text(reportConfig.ReportTitle, TextStyle.Default.FontSize(12).SemiBold());

                        // add date range
                        if (reportConfig.StartDate.HasValue && reportConfig.EndDate.HasValue)
                        {
                            stack.Item().Text(
                                  $"Period: {reportConfig.StartDate.Value:dd MMM yyyy} - {reportConfig.EndDate.Value:dd MMM yyyy}"
                             , TextStyle.Default.FontSize(10));
                        }
                        stack.Item().Text($"Generated on: {DateTime.Now:dd MMM yyyy HH:mm}",TextStyle.Default.FontSize(10));
                    });
                });

                column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
            });
        }

        // Flexible report content composition
        private void ComposeReportContent<T>(IContainer container, ReportConfig<T> reportConfig) where T : class
        {
            // Check if there are any data items
            if (reportConfig.Data == null || !reportConfig.Data.Any())
            {
                container.PaddingTop(20).Text("No data available for the selected period.").SemiBold();
                return;
            }

            // Use a Column to contain multiple elements
            container.Column(column =>
            {
                // Table section
                column.Item().PaddingTop(20).Table(table =>
                {
                    // Verify columns exist before defining them
                    if (reportConfig.Columns == null || !reportConfig.Columns.Any())
                    {
                        throw new InvalidOperationException("Report configuration must define at least one column");
                    }

                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var columnConfig in reportConfig.Columns)
                        {
                            if (columnConfig.Width.HasValue)
                                columns.ConstantColumn(columnConfig.Width.Value);
                            else
                                columns.RelativeColumn(columnConfig.RelativeWidth ?? 1);
                        }
                    });

                    // Header
                    table.Header(header =>
                    {
                        foreach (var columnConfig in reportConfig.Columns)
                        {
                            header.Cell().Background(Colors.LightBlue.Lighten3).Border(1)
                        .BorderColor(Colors.Black)
                        .PaddingVertical(5).PaddingLeft(5)
                                .Text(columnConfig.HeaderText).SemiBold().FontSize(10).FontColor(Colors.White);
                        }
                    });

                    // Data rows
                    foreach (var item in reportConfig.Data)
                    {
                        foreach (var columnConfig in reportConfig.Columns)
                        {
                            table.Cell().Border(1)
                        .BorderColor(Colors.Black)
                        .PaddingVertical(3).PaddingLeft(5).Text(
                                columnConfig.ValueSelector(item)?.ToString() ?? "N/A"
                            ).FontSize(10);
                        }
                    }
                });

                // Optional summary section
                if (reportConfig.SummaryItems != null && reportConfig.SummaryItems.Any())
                {
                    column.Item().PaddingTop(20).Row(row =>
                    {
                        row.RelativeColumn().Stack(stack =>
                        {
                            stack.Item().Text("Report Summary").SemiBold();

                            foreach (var summaryItem in reportConfig.SummaryItems)
                            {
                                stack.Item().Text(summaryItem);
                            }
                        });
                    });
                }
            });
        }
    }
}