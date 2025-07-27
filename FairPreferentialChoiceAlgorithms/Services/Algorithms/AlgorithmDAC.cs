using FairPreferentialChoiceAlgorithms.Models;
using System;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
    /// <summary>
    /// Deferred Acceptance: strategiesicher
    /// <para>Siehe <a href="https://en.wikipedia.org/wiki/Gale%E2%80%93Shapley_algorithm">DA auf Wikipedia</a></para>
    /// <para>Siehe <a href="https://people.duke.edu/~aa88/articles/BostonAERPP.pdf">Paper mit Abläufen von Algorithmen</a></para>
    /// </summary>
    public class AlgorithmDAC : IAssignmentAlgorithm
    {
        private readonly Random _random;
        public string Name => "DAC";

        public AlgorithmDAC(Random random)
        {
            _random = random;
        }

        public void Run(List<Course> courses, List<Student> students)
        {
            // 1. Kurspräferenzen generieren (auf Basis der Schülerpräferenzen)
            Dictionary<int, List<int>> coursePreferences = GeneriereCoursePreferences(students, courses);

            // Abbruchbedingung: Kein Schüler bewegt sich mehr zwischen Kurs und Schülerliste
            bool changed;

            do
            {
                changed = false;

                // 1. Alle unzugewiesenen Schüler bewerben sich bei ihrer aktuellen Präferenz (Kopie der Liste für sichere Iteration)
                foreach (var student in students.ToList())
                {
                    // Failsafe: Hat der Schüler keine Präferenzen oder das Ende seiner Präferenzen erreicht?
                    if (student.Preferences == null || student.Preferences.Count < 1)
                    {
                        continue;
                    }

                    // Failsafe: Existiert der Kurs?
                    var course = courses.FirstOrDefault(c => c.Id == student.Preferences.ElementAt(0));
                    if (course == null)
                    {
                        continue;
                    }

                    // 2. Schüler auf die Bewerberliste des Kurses versetzen:
                    // - Der Schüler Kann sich danach nicht erneut bei diesem Kurs bewerben
                    student.Preferences.RemoveAt(0);
                    course.Applicants.Add(student);
                    students.Remove(student);
                    changed = true;
                }

                // 3. Kurse wählen gemäß ihrer Präferenzen die besten Bewerber aus und lehnen den Rest ab
                foreach (var course in courses)
                {
                    // Bewerberliste nach Präferenzen seitens des Kurses sortieren
                    if (coursePreferences.TryGetValue(course.Id, out var preferredStudentIds))
                    {
                        course.Applicants = course.Applicants
                            .OrderBy(s =>
                            {
                                int index = preferredStudentIds.IndexOf(s.Id);
                                return index == -1 ? int.MaxValue : index; // unbekannte Schüler ganz hinten
                            })
                            .ToList();
                    }

                    // Failsafe: Hat der Kurs eine Kapazität?
                    int cap = course.Capacity ?? int.MaxValue;

                    // Alle Bewerber, die nicht mehr in den Kurs passen würden, werden zurückgewiesen
                    var rejectedStudents = course.Applicants.Skip(cap).ToList();
                    foreach (var student in rejectedStudents)
                    {
                        students.Add(student);
                        course.Applicants.Remove(student);
                        changed = true;
                    }
                }

            } while (changed);

            // 3. Schüler auf die Teilnehmerliste überführen
            foreach (var course in courses)
            {
                course.Participants = course.Applicants.ToList();
                course.Applicants.Clear();
            }
        }

        public Dictionary<int, List<int>> GeneriereCoursePreferences(List<Student> students, List<Course> courses)
        {
            var coursePreferences = new Dictionary<int, List<int>>();

            foreach (var course in courses)
            {
                // Studenten auswählen, die diesen course in ihrer Liste haben + Rang ermitteln
                var rankedStudents = students
                    .Where(s => s.Preferences.Contains(course.Id))
                    .Select(s => new
                    {
                        StudentId = s.Id,
                        Rang = s.Preferences.ToList().IndexOf(course.Id)
                    });

                // Nach Rang gruppieren, sortieren, dann shuffeln, dann flach zusammenfügen
                var orderedStudents = rankedStudents
                    .GroupBy(x => x.Rang)
                    .OrderBy(g => g.Key)
                    .SelectMany(g => g.OrderBy(_ => _random.Next()))
                    .Select(x => x.StudentId)
                    .ToList();

                // In das Dictionary eintragen
                coursePreferences[course.Id] = orderedStudents;
            }

            return coursePreferences;
        }
    }
}
