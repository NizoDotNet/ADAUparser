using System.IO;
using Newtonsoft.Json;

namespace Parser
{
    public static class WriteToJson
    {
        public static void WriteStudentInfo(List<Student> students)
        {
            File.WriteAllText($"IT.json", JsonConvert.SerializeObject(students));
        }
    }
}
