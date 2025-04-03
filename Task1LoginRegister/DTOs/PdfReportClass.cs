using Task1LoginRegister.Interfaces;

namespace Task1LoginRegister.DTOs
{
    public class ReportConfig<T> :IReportConfig where T : class
    {
        public string ReportTitle { get; set; } = "Report";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsLandscape { get; set; } = false;
        public IEnumerable<T>? Data { get; set; }
        public List<ReportColumn<T>>? Columns { get; set; }
        public List<string>? SummaryItems { get; set; }
    }

    public class ReportColumn<T>
    {
        public string HeaderText { get; set; } = string.Empty;
        public Func<T, object?> ValueSelector { get; set; } = null!;
        public float? Width { get; set; }
        public int? RelativeWidth { get; set; }
    }
}
