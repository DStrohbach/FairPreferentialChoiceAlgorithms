using FairPreferentialChoiceAlgorithms.Models;
using FairPreferentialChoiceAlgorithms.Models.Datasets;
using FairPreferentialChoiceAlgorithms.Services.Algorithms;
using FairPreferentialChoiceAlgorithms.Services.Heuristics;

namespace FairPreferentialChoiceAlgorithms.Services.Heuristics
{
    public class HeuristicNone : IAssignmentHeuristic
    {

        public string Name => "STD";

        /// <summary>
        /// Führt den angegebenen Zuteilungsalgorithmus auf einem InputDataset aus und gibt das Ergebnis als AssignmentDataset zurück.
        /// </summary>
        public AssignmentDataset CreateAssignment(InputDataset setup, IAssignmentAlgorithm algorithm, int deletedCourses = 0)
        {
            // 1. Daten initialisieren
            string heuristicName = this.Name;
            string algorithmName = algorithm.Name;
            string inputDataName = setup.Name;
            List<Course> courses = HeuristicUtilities.InitializeCourses(setup.Courses);
            List<Student> students = HeuristicUtilities.InitializeStudents(setup.Preferences);

            // -- Failsafe
            if (!students.Any() || students.All(s => s.Preferences == null || s.Preferences.Count == 0))
            {
                throw new InvalidOperationException("Keine gültigen Präferenzen im Eingabedatensatz vorhanden.");
            }

            // 2. Algorithmus Zuteilung vornehmen lassen
            algorithm.Run(courses, students);

            // 3. Analyse der Zuteilung
            AssignmentDataset result = new AssignmentDataset(setup, HeuristicUtilities.CreateResultDictionary(courses, students), algorithmName, heuristicName);
            return result;
        }
    }
}
