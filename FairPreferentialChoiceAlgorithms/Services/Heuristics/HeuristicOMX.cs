using FairPreferentialChoiceAlgorithms.Models;
using FairPreferentialChoiceAlgorithms.Models.Datasets;
using FairPreferentialChoiceAlgorithms.Services.Algorithms;

namespace FairPreferentialChoiceAlgorithms.Services.Heuristics
{
    /// <summary>
    /// Zufallsausgleich + Mindestbelegung
    /// </summary>
    public class HeuristicOMX : IAssignmentHeuristic
    {
        public string Name => $"OM{MaxDeletedCourses}";
        public string Description => "Optimierung von Mindestbelegung unter Streichen von maximal X Kursen.";

        public int? MaxDeletedCourses;

        public HeuristicOMX(int? maxDeletedCourses)
        {
            MaxDeletedCourses = maxDeletedCourses;
        }

        /// <summary>
        /// Führt den angegebenen Zuteilungsalgorithmus auf einem InputDataset aus und gibt das Ergebnis als AssignmentDataset zurück.
        /// </summary>
        public AssignmentDataset CreateAssignment(InputDataset setup, IAssignmentAlgorithm algorithm, int deletedCourses = 0)
        {
            // Eingabedaten initialisieren
            string heuristicName = this.Name;
            string algorithmName = algorithm.Name;
            string inputDataName = setup.Name;
            List<Course> courses = new();
            List<Student> students = new();

            // 1. Zufallsausgleich: Wiederhole Zuteilung maximal X-mal, um ggf. unvollständige Zuteilungen zu kompensieren
            for (int i=0; i<20; i++)
            {
                // 2. Daten initialisieren (Originale kopieren)
                courses = HeuristicUtilities.InitializeCourses(setup.Courses);
                students = HeuristicUtilities.InitializeStudents(setup.Preferences);

                // -- Failsafe: Gibt keine Schüler oder haben sie keine Präferenzen?
                if (!students.Any() || students.All(s => s.Preferences == null || s.Preferences.Count == 0))
                {
                    throw new InvalidOperationException("Keine gültigen Präferenzen im Eingabedatensatz vorhanden.");
                }

                // 3. Algorithmus Zuteilung vornehmen lassen
                algorithm.Run(courses, students);

                // 4. Es konnten alle Schüler verteilt werden, beende die Schleife vorzeitig mit diesem Ergebnis
                if (!students.Any())
                {
                    break;
                }
            }
            
            // 5. Analyse der Zuteilung
            // -- Gibt es noch Schüler ohne Zuteilung? (X Schleifen des Zufallsausgleichs konnten kein gutes Ergebnis produzieren)
            if (students.Any())
            {
                // Ja, beende mit unvollständiger Liste
                return new AssignmentDataset(setup, HeuristicUtilities.CreateResultDictionary(courses, students), algorithmName, heuristicName);
            } else
            {
                // 6. Maximale Anzahl der Runden rekursiven Kursstreichens erreicht?
                if (!MaxDeletedCourses.HasValue || (MaxDeletedCourses.HasValue && deletedCourses < MaxDeletedCourses.Value))
                {
                    // Nein, Liste könnte also eventuell um unterbelegte Kurse gekürzt werden
                    // Suche alle Kurse, die ihre Mindestbelegung nicht erreichen konnten, aber auf den Präferenzlisten erwähnt werden.
                    // (Kurse, die auf den Listen nicht erwähnt werden, erhalten ohnehin keinen Schüler)
                    List<Course> underoccupiedCourses = courses
                        .Where(c =>
                            c.MinimumStudents != null &&
                            c.Participants.Count < c.MinimumStudents &&
                            setup.Preferences.Any(prefs => prefs.Contains(c.Id))
                        )
                        .OrderBy(c => c.Participants.Count)
                        .ToList();

                    // -- Gibt es Kurse, die unterbelegt sind, die auch auf jemandes Zettel stehen?
                    if (underoccupiedCourses.Any())
                    {
                        // Ja, wähle den Kurs mit der geringsten Belegung,
                        int courseToRemove = underoccupiedCourses.First().Id;
                        // streiche diesen Kurs aus den Präferenzen der Schüler,
                        InputDataset editedSetup = HeuristicUtilities.RemoveCourseFromSetup(setup, courseToRemove);
                        // probiere dann, eine Zuteilung der reduzierten Daten vorzunehmen (rekursiver Aufruf)
                        AssignmentDataset result = CreateAssignment(editedSetup, algorithm, deletedCourses + 1);

                        // -- War die Zuteilung ein Misserfolg (Gibt es im Schüler-Kurs-Dictionary Einträge mit null)?
                        if (result.Students.Any(v => v.AssignedCourse == null))
                        {
                            // Ja, verwirf den Versuch und gib die eigentliche Zuteilung zurück
                            return new AssignmentDataset(setup, HeuristicUtilities.CreateResultDictionary(courses, students), algorithmName, heuristicName, deletedCourses);
                        }
                        else
                        {
                            // Nein, gib die neue Zuteilung zurück, aber mit den vollständigen Präferenzen
                            result.Students = result.Students
                            .Select(s => (
                                s.Id,
                                s.AssignedCourse,
                                Preferences: setup.Preferences[s.Id]  // Gekürzte Präferenzen mit Originalen überschreiben
                            ))
                            .ToList();

                            return result;
                        }
                    }
                    else
                    {
                        // Nein, dies ist die beste Lösung Lösung!
                        return new AssignmentDataset(setup, HeuristicUtilities.CreateResultDictionary(courses, students), algorithmName, heuristicName, deletedCourses);
                    }
                } else
                {
                    // Ja, weiter darf nicht gesucht werden. Dies ist die vorläufig beste Lösung.
                    return new AssignmentDataset(setup, HeuristicUtilities.CreateResultDictionary(courses, students), algorithmName, heuristicName, deletedCourses);
                }
            }    
        }
    }
}
