namespace Task1LoginRegister.Interfaces
{
    public interface IReportConfig
    {
        string ReportTitle { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
    }
}
