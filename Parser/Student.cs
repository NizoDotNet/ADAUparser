using System;


namespace Parser
{
    public class Student : IComparable
    {
        public string StudentFullName { get; init; }
        public string Speciality { get; init; }
        public bool PassedAllExams { get; set; }
        public int ID { get; init; }
        public List<Subject> Subjects { get; set; }

        public double AVG { get; set; }
        public Student()
        {
            Subjects = new List<Subject>();
            PassedAllExams = true;
        }

        public int CompareTo(object? obj)
        {
            if(obj is Student t)
                return this.AVG.CompareTo(t.AVG);
            throw new ArgumentException("This is not Students");
        }
    }
}
