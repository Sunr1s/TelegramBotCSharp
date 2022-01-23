using System.Data.Entity;

namespace Bot
{
    public class Mens_Short_Course
    {

        public int Id { get; set; }
        public string Distance { get; set; }
        public string TimeRecord { get; set; }
        public string NameRecord { get; set; }
        public string DataSetup { get; set; }
     
    }
  
    public class Mens_Short_CourseContext : DbContext
    {
        private const string connectionString = @"Data Source =DESKTOP-6ENFBPN\SQLEXPRESS01;Initial Catalog = BestResults;;Integrated Security=True";

        public Mens_Short_CourseContext() : base(connectionString)
        { }

        public DbSet<Mens_Short_Course> Mens_Short_Courses { get; set; }
    }
}
