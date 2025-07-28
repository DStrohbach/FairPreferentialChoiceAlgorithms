using FairPreferentialChoiceAlgorithms.Models;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
/// <summary>
/// Random Serial Dictotarship: strategiesicher
    /// <para>Siehe <a href="https://en.wikipedia.org/wiki/Dictatorship_mechanism">Wikipedia: Dictatorship Mechanism</a></para>
    /// <para>Siehe <a href="https://en.wikipedia.org/wiki/Random_priority_item_allocation">Wikipedia: Random Serial Dictatorship</a></para>
/// </summary>
public class AlgorithmRSD : IAssignmentAlgorithm
{
    private readonly Random _random;
    public string Name => "RSD";

    public AlgorithmRSD(Random random)
    {
        _random = random;
    }

    public void Run(List<Course> courses, List<Student> students)
    {
        // 1. Reihenfolge der Sch�ler zuf�llig anordnen
        List<Student> studentsInRandomOrder = students.ToList();
        studentsInRandomOrder.Shuffle(_random);

        // 2. Iteriere �ber alle Sch�ler
        foreach (Student student in studentsInRandomOrder)
        {
            // 3. Iteriere �ber die geordneten Pr�ferenzen des Sch�lers
            foreach (int preference in student.Preferences)
            {
                // 4. Suche den zugeh�rigen Kurs
                Course? course = courses.FirstOrDefault(course => course.Id == preference);

                // 5. Falls der Kurs existiert und noch einen Platz hat, teile den Sch�ler zu
                if (course != null && course.Participants.Count < course.Capacity)
                {
                    course.Participants.Add(student);
                    students.Remove(student);
                    break; // Nur eine Zuweisung pro Sch�ler!
                }
            }
        }
    }
}
}
