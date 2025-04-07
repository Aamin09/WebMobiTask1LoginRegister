namespace Task1LoginRegister.Services
{
    public class DateRangeService
    {

        public DateRange GetDatesRange(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate?.Date ?? DateTime.Today.AddDays(-30);
            var end = endDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

            return new DateRange
            {
                StartDate = start,
                EndDate = end
            };
        }
    }

    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
