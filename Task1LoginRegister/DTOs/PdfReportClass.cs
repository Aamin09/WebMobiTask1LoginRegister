using Task1LoginRegister.Interfaces;

namespace Task1LoginRegister.DTOs
{
    public class ReportConfig<T> :IReportConfig where T : class
    {
        public string ReportTitle { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<T> Data { get; set; }
        public List<ReportColumn<T>> Columns { get; set; }
        public List<string> SummaryItems { get; set; }
    }

    public class ReportColumn<T>
    {
        public string HeaderText { get; set; }
        public Func<T,object> ValueSelector { get; set; }
        public float? Width { get; set; }
        public float? RelativeWidth { get; set; }
    }
}
