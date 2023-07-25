using Parser;
using Parser.Async;

ADAUParser a = new ADAUParser(30);
ParseAsync test = new();



int id = 0;
int count = 0;

bool isId = false, isCount = false;
while (!isId && !isCount)
{
    Console.Write("Type ID: ");
    isId = int.TryParse(Console.ReadLine(), out id);
    Console.Write("Type Count: ");
    isCount = int.TryParse(Console.ReadLine(), out count);
}


var studentsArr = a.ParseAll(id, count);
WriteToJson.WriteStudentInfo(studentsArr);
//Display.Show(studentsArr);


Console.ReadKey();