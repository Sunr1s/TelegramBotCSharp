using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Bot
{

    public class CallculatePoints
    {

        static double UserTime, RecordTime;
        static string recordTimeFromBD;


        public static string Callculatorpoints(string sex, string cousre, string distance, string time)
        {                                                                                         //                B     3
                                                                                                  // P = 1000 x  ( ---- ) 
                                                                                                  //                T
            UserTime = SplitTime(time); // Translete string to seconds for user time

            recordTimeFromBD = TakeRecordTimeFromBD(sex, cousre, distance); // Take standart time from Base

            RecordTime = SplitTime(recordTimeFromBD); // Translete string to seconds for record time

            Console.WriteLine("!!!" + UserTime + "  " + RecordTime); // Console Logs

            string RecordBack = FuncWorkWithDb(sex, cousre, distance);  // Take Record info from Base

            double P = 1000 * Math.Pow((RecordTime / UserTime), 3);  // Calculate Points use formul

            return RecordBack + "\nВаше количество поинтов: " + Convert.ToInt32(P); // Return record info and points
        }
        // Func Take info record from Db
        static string FuncWorkWithDb(string sex, string cousre, string distance)
        {
            string BackInfoDb = "";
            // Separete Table in Db
            if (sex == "M" && cousre == "25") 
            {
                Mens_Short_CourseContext db = new Mens_Short_CourseContext();

                var customer = db.Mens_Short_Courses.Where(c => c.Distance == distance);
                foreach (var name in customer)
                    BackInfoDb = "\nНа дистанции: " + name.Distance +
                                 "\nВладелец рекорда: мира " + name.NameRecord +
                                 "\nC результатом: " + name.TimeRecord +
                                 "\nУстановленым: " + name.DataSetup;
                return BackInfoDb;
            }
            else if (sex == "M" && cousre == "50")
            {
                Mens_Long_CourseContext db = new Mens_Long_CourseContext();

                var customer = db.Mens_Long_Courses.Where(c => c.Distance == distance);
                foreach (var name in customer)
                    BackInfoDb = "\nНа дистанции: " + name.Distance +
                                 "\nВладелец рекорда мира: " + name.NameRecord +
                                 "\nC результатом: " + name.TimeRecord +
                                 "\nУстановленым: " + name.DataSetup;
                return BackInfoDb;
            }
            else if (sex == "Ж" && cousre == "25")
            {
                Womens_Short_CourseContext db = new Womens_Short_CourseContext();

                var customer = db.Womens_Short_Courses.Where(c => c.Distance == distance);
                foreach (var name in customer)
                    BackInfoDb = "\nНа дистанции: " + name.Distance +
                                 "\nВладелец рекорда: мира " + name.NameRecord +
                                 "\nC результатом: " + name.TimeRecord +
                                 "\nУстановленым: " + name.DataSetup;
                return BackInfoDb;
            }
            else if (sex == "Ж" && cousre == "50")
            {
                Womens_Long_CourseContext db = new Womens_Long_CourseContext();

                var customer = db.Womens_Long_Courses.Where(c => c.Distance == distance);
                foreach (var name in customer)
                    BackInfoDb = "\nНа дистанции: " + name.Distance +
                                 "\nВладелец рекорда мира: " + name.NameRecord +
                                 "\nC результатом: " + name.TimeRecord +
                                 "\nУстановленым: " + name.DataSetup;
                return BackInfoDb;
            }
            else // If table or result not found
                return "Случилась ошибка или результат не найден!";
        }
        // Func For normalize string to seconds
        static double SplitTime(string time)
        {
            var words = time.Split(new char[] { '.', ',' }); // init separator
            double Time = 0; // Time var
            int k = 0;  // words amount counter var
            if (words.Length == 2) // if no mitutes go to next word
                k++;
            for (int i = 0; k <= words.Length; i++, k++) // Going to all words cucle
            {

                if (k == 0) // if Have  minutes
                    Time = Convert.ToDouble(words[i]) * 60.0;   //each mitutes  * 60
                else if (k == 1) // + seconds
                    Time = Time + Convert.ToDouble(words[i]);
                else if (k == 2) // + miliseconds 100 mili == 1 sec
                    Time = Time + Convert.ToDouble(words[i]) / 100;
                else
                    i--;   // Decrement word counter

            }
            return Time;
        }
        // Same func to read tale in bd and return just record time string
        static string TakeRecordTimeFromBD(string sex, string cousre, string distance)
        {
            if (sex == "M" && cousre == "25")
            {
                Mens_Short_CourseContext db = new Mens_Short_CourseContext();

                var RecordTimeFromBd = db.Mens_Short_Courses.FirstOrDefault(c => c.Distance == distance);

                return RecordTimeFromBd.TimeRecord;
            }
            if (sex == "M" && cousre == "50")
            {
                Mens_Long_CourseContext db = new Mens_Long_CourseContext();

                var RecordTimeFromBd = db.Mens_Long_Courses.FirstOrDefault(c => c.Distance == distance);

                return RecordTimeFromBd.TimeRecord;
            }
            if (sex == "Ж" && cousre == "25")
            {
                Womens_Short_CourseContext db = new Womens_Short_CourseContext();

                var RecordTimeFromBd = db.Womens_Short_Courses.FirstOrDefault(c => c.Distance == distance);

                return RecordTimeFromBd.TimeRecord;
            }
            if (sex == "Ж" && cousre == "50")
            {
                Womens_Long_CourseContext db = new Womens_Long_CourseContext();

                var RecordTimeFromBd = db.Womens_Long_Courses.FirstOrDefault(c => c.Distance == distance);

                return RecordTimeFromBd.TimeRecord;
            }
            else
                return "Случилась ошибка или результат не найден!";

        }

    }
}
