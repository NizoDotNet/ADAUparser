using HtmlAgilityPack;
using System;


namespace Parser.Async
{
    public class ParseAsync
    {
        public const string URL = "http://tnis.adau.edu.az/forparents/marks.php?studentid=";
        Student student;
        Subject subject;
        List<Student> students = new List<Student>();
        public async Task<Student> ParseAs(int id)
        {
            await Task.Run(() =>
            {

                HtmlWeb htmlWeb = new HtmlWeb();
                var nodeP = htmlWeb.Load(URL + id);
                int subjectCount = 0;
                student = new Student()
                {
                    StudentFullName = nodeP.DocumentNode.SelectSingleNode("//section[@class='marks']/h1").InnerHtml,
                    Speciality = nodeP.DocumentNode.SelectSingleNode("//section[@class='marks']/h2").InnerHtml,
                    ID = id
                };

                foreach (var x in nodeP.DocumentNode.SelectNodes("//table[@class='table table-striped table-bordered']/tbody/tr"))
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
            });
            return student;
        }
        public async Task ParseAllAs(int startID, int studentCount)
        {
            for (int i = 0; i < studentCount; i++)
            {
                students.Add(ParseAs(startID + i).Result);
            }
        }

        

        public void Display()
        {
            foreach (var student in this.students)
            {
                Console.WriteLine(student.StudentFullName);
            }
        }
    }
}
