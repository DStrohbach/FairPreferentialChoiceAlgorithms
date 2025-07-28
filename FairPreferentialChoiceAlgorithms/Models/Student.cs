namespace FairPreferentialChoiceAlgorithms.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name => $"S-{Id:D3}";
        public List<int> Preferences { get; set; } = new List<int>();
        public Student(int id, List<int> preferences)
        {
            Id = id;
            Preferences = preferences;
        }
    }
}

