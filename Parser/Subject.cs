namespace Parser
{
    public class Subject
    {
        public string SubjectName { get; set; }
        public int EnterScore { get; set; }
        public int ExitScore { get; set; }
        public int FullGrade { get; set; }
        public int Credit { get; set; }
        public string GradeWithChar { get; set; }
        public bool Failed { get; set; } = false;
        public bool NoGrade { get; set; } = false;

        public string Semester { get; set; }

    }
}
