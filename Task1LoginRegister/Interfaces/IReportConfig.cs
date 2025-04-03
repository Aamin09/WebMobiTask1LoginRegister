namespace Task1LoginRegister.Interfaces
{
    public interface IReportConfig
    {
        string ReportTitle { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        bool IsLandscape { get; set; }
    }
}
