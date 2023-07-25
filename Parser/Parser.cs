using HtmlAgilityPack;
using System.Xml.Linq;

namespace Parser
{
    public class ADAUParser
    {
        public const string URL = "http://tnis.adau.edu.az/forparents/marks.php?studentid=";
        Student student;
        Subject subject;
        List<Student> students = new List<Student>();
        public Student[] stundetsArray;
        public string Semester { get; set; }
        public ADAUParser() { }

        public ADAUParser(int count)
        {
            stundetsArray = new Student[count];
        }
        public ADAUParser(string semester)
        {
            Semester = semester;
        }

        public ADAUParser(string semester, int count)
        {
            Semester = semester;
            stundetsArray = new Student[count];
        }

        public Student Parse(int id)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            var nodeP = htmlWeb.Load(URL + id);
            int subjectCount = 0;
            student = new Student()
            {
                StudentFullName = nodeP.DocumentNode.SelectSingleNode("//section[@class='marks']/h1").InnerText,
                Speciality = nodeP.DocumentNode.SelectSingleNode("//section[@class='marks']/h2").InnerText,
                ID = id
            };

            foreach(var x in nodeP.DocumentNode.SelectNodes("//table[@class='table table-striped table-bordered']/tbody/tr"))
            {
                if (x.InnerText != "") subjectCount++;
            }

            for (int j = 1; j <= subjectCount; j++)
            {
                try
                {

                    subject = new Subject();
                    subject.SubjectName = nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[2]").InnerText;
                    subject.GradeWithChar = nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[8]").InnerText;
                    subject.Credit = int.Parse(nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[4]").InnerText);
                    subject.Semester = nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[3]").InnerText;
                    int score;
                    bool check = int.TryParse(nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[5]").InnerText, out score);
                    subject.EnterScore = check ? score : 0;
                    if (nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[6]").InnerText != "-")
                    {
                        check = int.TryParse(nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[6]").InnerText, out score);
                        subject.ExitScore = check ? score : 0;
                        subject.FullGrade = subject.EnterScore + subject.ExitScore;
                        if (subject.FullGrade < 51 || subject.ExitScore < 17)
                        {
                            subject.Failed = true;
                            check = int.TryParse(nodeP.DocumentNode.SelectSingleNode($"//table[@class='table table-striped table-bordered']/tbody/tr[{j}]/td[10]").InnerText, out score);
                            subject.ExitScore = check ? score : 0;
                            subject.FullGrade = subject.EnterScore + subject.ExitScore;
                        }
                    }
                    else subject.NoGrade = true;
                    student.Subjects.Add(subject);
                    student.PassedAllExams = !subject.Failed && student.PassedAllExams ? true : false;
                }
                catch 
                {
                    continue;
                }
                

            }
            if (Semester != null) student.AVG = AVGPoint(student, Semester);
            else student.AVG = AVGPoint(student);
            return student;
        }
        public List<Student> ParseAll(int startID, int studentCount)
        {
            for (int i = 0; i < studentCount; i++)
            {    
                students.Add(Parse(startID + i));
                
            }     
            return students;
        }

        public Student[] ParseAllToArray(int startID)
        {
            for(int i = 0; i < stundetsArray.Length; i++)
            {
                stundetsArray[i] = Parse(startID + i);
            }
            return stundetsArray;
        }
        public double AVGPoint(Student student)
        {
            double res = 0;
            int credit = 0;
            foreach (var subject in student.Subjects)
            {
                if(!subject.NoGrade)
                {
                    res += subject.Credit * subject.FullGrade;
                    credit += subject.Credit;
                }
            }
            return res / credit;
        }
        public double AVGPoint(Student student, string semester)
        {
            double res = 0;
            int credit = 0;
            foreach (var subject in student.Subjects)
            {
                if (!subject.NoGrade && subject.Semester == semester)
                {
                    res += subject.Credit * subject.FullGrade;
                    credit += subject.Credit;
                }
            }
            return res / credit;
        }
        public Dictionary<string, double> GetALLAVG(List<Student> students, string semester, string fileName)
        {
            var res = new Dictionary<string, double>();
            foreach (var student in students)
            {
                res.Add(student.StudentFullName, AVGPoint(student, semester));
            }
            foreach(var c in res)
            {
                Console.WriteLine($"{c.Key} - {c.Value}");
            }
            return res;
        }
        
    }

}
