using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Display
    {
        public static void Show(List<Student> students)
        {
            ConsoleColor currColor = Console.ForegroundColor;
            ConsoleColor Passed = ConsoleColor.Green;
            ConsoleColor Failed = ConsoleColor.Red;
            ConsoleColor NoGrade = ConsoleColor.Yellow;

            int index = 0;
            foreach (var student in students)
            {
                Console.WriteLine($"{student.ID} {index + 1} {student.StudentFullName}");
                index++;
            }
            bool chosed = false;
            int chosenIndex = 0;
            while (!chosed)
            {
                chosed = int.TryParse(Console.ReadLine(), out chosenIndex);
            }
            foreach (var subject in students[chosenIndex - 1].Subjects)
            {
                if (subject.NoGrade)
                    Console.ForegroundColor = NoGrade;
                else if (subject.Failed)
                    Console.ForegroundColor = Failed;
                else
                    Console.ForegroundColor = Passed;

                Console.WriteLine(subject.SubjectName);
                Console.WriteLine($"--> Credit: {subject.Credit}");
                Console.WriteLine($"--> Enter Score: {subject.EnterScore}");
                Console.WriteLine($"--> Exit Socore: {subject.ExitScore}");
                Console.WriteLine($"--> Full Grade: {subject.FullGrade}");
                Console.WriteLine($"--> Grade With Char: {subject.GradeWithChar}");
                Console.ForegroundColor = currColor;
            }
            Console.WriteLine(students[chosenIndex - 1].AVG);
        }
        public static void SortByAvgAndDisplay(Student[] students)
        {
            Array.Sort(students);
            for (int i = 0; i < students.Length; i++)
            {
                Console.WriteLine($"{students[i].StudentFullName} - {students[i].AVG}");
            }
        }
    }
}
