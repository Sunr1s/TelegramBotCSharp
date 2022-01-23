using System.Data.Entity;

namespace Bot
{
    public class Womens_Long_Course
    {

        public int Id { get; set; }
        public string Distance { get; set; }
        public string TimeRecord { get; set; }
        public string NameRecord { get; set; }
        public string DataSetup { get; set; }

    }

    public class Womens_Long_CourseContext : DbContext
    {
        private const string connectionString = @"Data Source =DESKTOP-6ENFBPN\SQLEXPRESS01;Initial Catalog = BestResults;;Integrated Security=True";

        public Womens_Long_CourseContext() : base(connectionString)
        { }

        public DbSet<Womens_Long_Course> Womens_Long_Courses { get; set; }
    }
}
