using BlazorBootstrap;

namespace FairPreferentialChoiceAlgorithms.Models.Datasets
{
    public class AssignmentDataset
    {
        /// <summary>
        /// Bezeichnung der verwendeten Heuristik.
        /// </summary>
        public string HeuristicName { get; set; }
        /// <summary>
        /// Bezeichnung des verwendeten Algorithmus.
        /// </summary>
        public string AlgorithmName { get; set; }
        /// <summary>
        /// Typisierung der verwendeten Eingabedaten.
        /// </summary>
        public string InputDatasetType { get; set; }
        /// <summary>
        /// Bezeichnung der verwendeten Eingabedaten.
        /// </summary>
        public string InputDatasetName { get; set; }
        /// <summary>
        /// Liste der verfügbaren Kurse (Id, Kapazität, Minimum).
        /// </summary>
        public List<(int Id, int? Capacity, int? Minimum)> Courses { get; set; } = new();
        /// <summary>
        /// Liste der verfügbaren Kurse (Id, Kapazität, Minimum).
        /// </summary>
        public int DeletedCoursesCount { get; set; }
        /// <summary>
        /// Schülerdaten mit Präferenzlisten (Kurs-IDs) und zugewiesenem Kurs.
        /// </summary>
        public List<(int Id, int? AssignedCourse, List<int> Preferences)> Students { get; set; } = new();

        // Attribute für die Anzeige in Tabellen
        public int UnassignedStudentsCount => Students.Count(s => s.AssignedCourse == null);

        public (int? Participants, int? Capacity, int? Minimum) GetCourseStatByCourseId(int index)
        {
            if (index < 0 || index >= Courses.Count)
                return (null, null, null);

            var course = Courses[index];
            int participants = Students.Count(s => s.AssignedCourse == course.Id);
            return (participants, course.Capacity, course.Minimum);
        }

        public List<(int Id, List<int> Preferences)> GetStudentsByCourseId(int? courseId)
        {
            if (!courseId.HasValue)
            {
                return Students
                .Where(s => s.AssignedCourse == null)
                .Select(s => (s.Id, s.Preferences))
                .ToList();
            } else
            {
                return Students
                .Where(s => s.AssignedCourse == courseId)
                .Select(s => (s.Id, s.Preferences))
                .ToList();
            }
                
        }

        public AssignmentDataset() { }

        public AssignmentDataset(InputDataset inputDataset, Dictionary<int, int?> assignments, string algorithmName, string heuristicName, int deletedCoursesCount = 0)
        {
            // Kopiere alle Kurse aus den Eingabedaten, ergänze Ids
            Courses = inputDataset.Courses
                .Select((course, index) => (index, course.Capacity, course.Minimum))
                .ToList();

            // Kopiere alle Schülerpräferenzen aus den Eingabedaten, ergänze Ids und zugewiesenen Kurs
            Students = inputDataset.Preferences
                .Select((prefs, index) => (
                    Id: index,
                    AssignedCourse: assignments.TryGetValue(index, out var courseId) ? courseId : null,
                    Preferences: prefs
                ))
                .ToList();

            HeuristicName = heuristicName;
            AlgorithmName = algorithmName;
            InputDatasetType = inputDataset.Type;
            InputDatasetName = inputDataset.Name;
            DeletedCoursesCount = deletedCoursesCount;
        }
    }
}
