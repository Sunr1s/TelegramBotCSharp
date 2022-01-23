using System.Data.Entity;

namespace Bot
{
    public class Womens_Short_Course
    {

        public int Id { get; set; }
        public string Distance { get; set; }
        public string TimeRecord { get; set; }
        public string NameRecord { get; set; }
        public string DataSetup { get; set; }

    }

    public class Womens_Short_CourseContext : DbContext
    {
        private const string connectionString = @"Data Source =DESKTOP-6ENFBPN\SQLEXPRESS01;Initial Catalog = BestResults;;Integrated Security=True";

        public Womens_Short_CourseContext() : base(connectionString)
        { }

        public DbSet<Womens_Short_Course> Womens_Short_Courses { get; set; }
    }
}
