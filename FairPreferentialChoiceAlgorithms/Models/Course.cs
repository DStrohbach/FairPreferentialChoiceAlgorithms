using System.Diagnostics;

namespace FairPreferentialChoiceAlgorithms.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name => $"Kurs-{Id}";
        public int? Capacity { get; set; }
        public int? MinimumStudents { get; set; }
        public List<Student> Participants { get; set; } = new List<Student>();
        public List<Student> Applicants { get; set; } = new List<Student>(); // Für Boston, DA
        public List<int> RankedStudentIds { get; set; } = new(); // Für DA

        public Course(int id, int? capacity, int? minimumStudents)
        {
            Id = id;
            Capacity = capacity;
            MinimumStudents = minimumStudents;
        }
    }
}
