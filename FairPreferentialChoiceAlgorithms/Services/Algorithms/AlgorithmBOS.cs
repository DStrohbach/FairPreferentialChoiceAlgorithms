using FairPreferentialChoiceAlgorithms.Models;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
    /// <summary>
    /// Boston Algorithmus: nicht strategiesicher
    /// <para>Siehe <a href="http://fmwww.bc.edu/EC-P/wp729.pdf">Paper mit Details</a></para>
    /// <para>Siehe <a href="https://people.duke.edu/~aa88/articles/BostonAERPP.pdf">Paper mit Abläufen von Algorithmen</a></para>
    /// </summary>
    public class AlgorithmBOS : IAssignmentAlgorithm
    {
        private readonly Random _random;
        public string Name => "BOS";

        public AlgorithmBOS(Random random)
        {
            _random = random;
        }

        public void Run(List<Course> courses, List<Student> students)
        {
            // Maximal mögliche Runden = maximale Präferenzlänge
            int maxRounds = students.Max(s => s.Preferences.Count);

            // 1. Pro Runde:
            for (int k = 0; k < maxRounds; k++)
            {
                // 2. Iteriere über alle Kurse
                foreach (var course in courses)
                {
                    // 3. Nimm Schüler mit Kurs k an k-ter Stelle in deine Bewerberliste
                    course.Applicants = students
                        .Where(s => k < s.Preferences.Count && s.Preferences[k] == course.Id)
                        .ToList();
                    // 4. Mische die Liste
                    course.Applicants.Shuffle(_random);

                    // 5. Iteriere über die Bewerber
                    foreach (var student in course.Applicants)
                    {
                        // Hat der Kurs noch Platz?
                        if (course.Participants.Count < course.Capacity)
                        {
                            // Ja, Schüler in Kurs verschieben
                            course.Participants.Add(student);
                            students.Remove(student);
                        }
                        else
                        {
                            // Nein, Kurs ist voll
                            break;
                        }
                    }
                }
            }
        }
    }
}
