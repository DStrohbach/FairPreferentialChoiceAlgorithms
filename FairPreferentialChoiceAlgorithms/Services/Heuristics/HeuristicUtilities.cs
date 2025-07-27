using FairPreferentialChoiceAlgorithms.Models;
using FairPreferentialChoiceAlgorithms.Models.Datasets;

namespace FairPreferentialChoiceAlgorithms.Services.Heuristics
{
    public class HeuristicUtilities
    {
        /// <summary>
        /// Init-Funktion: Erstellt eine Liste von Course-Objekten (mit Ids) aus den rohen Kursdaten.
        /// </summary>
        public static List<Course> InitializeCourses(List<CourseData> courseData)
        {
            return courseData
                .Select((c, index) => new Course(index, c.Capacity, c.Minimum))
                .ToList();
        }

        /// <summary>
        /// Init-Funktion: Erstellt eine Liste von Student-Objekten (mit Ids) aus den Präferenzdaten.
        /// </summary>
        public static List<Student> InitializeStudents(List<List<int>> studentData)
        {
            return studentData
                .Select((preferences, index) => new Student(index, preferences.ToList()))
                .ToList();
        }

        /// <summary>
        /// Util-Funktion: Entfernt einen Kurs aus den Präferenzen der Schüler (nicht aus der Kursliste, damit man die vollständigen Kurse noch kennt!) und gibt eine neue InputDataset-Kopie zurück.
        /// </summary>
        public static InputDataset RemoveCourseFromSetup(InputDataset initialSetup, int courseId)
        {
            // 1. Neue Instanz erzeugen
            InputDataset editedSetup = new InputDataset
            {
                Type = initialSetup.Type,
                Name = initialSetup.Name + $"-{courseId}",

                // 2. Kurse belassen, um Ids nicht zu verfälschen
                Courses = initialSetup.Courses,

                // 3. Präferenzen kürzen
                Preferences = initialSetup.Preferences
                    .Select(prefs => prefs.Where(p => p != courseId).ToList())
                    .ToList()
            };

            return editedSetup;
        }

        /// <summary>
        /// Util-Funktion: Erstellt aus der aktuellen Kursliste ein Dictionary mit Schüler-IDs und zugewiesenen Kursen (oder null).
        /// </summary>
        public static Dictionary<int, int?> CreateResultDictionary(List<Course> courses, List<Student> unassignedStudents)
        {
            Dictionary<int, int?> studentAssignments = new Dictionary<int, int?>();
            foreach (var course in courses)
            {
                foreach (var student in course.Participants)
                {
                    studentAssignments.Add(student.Id, course.Id);
                }
            }
            foreach (var student in unassignedStudents)
            {
                studentAssignments.Add(student.Id, null);
            }

            return studentAssignments;
        }
    }
}
