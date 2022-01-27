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


        public Womens_Short_CourseContext() : base("DefaultConnection")
        { }

        public DbSet<Womens_Short_Course> Womens_Short_Courses { get; set; }
    }
}
