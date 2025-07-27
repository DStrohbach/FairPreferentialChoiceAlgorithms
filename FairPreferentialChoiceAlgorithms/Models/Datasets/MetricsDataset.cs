
/*
 Anmerkung des Authors, 25.07.2025: Entschuldigen Sie die nachfolgenden Monster-Funktionen. Es waren anfangs alles einzelne Funktionen,
 die ich zugunsten von Rechenzeit und auch einer kompakteren Darstellung in der Thesis zusammengefügt habe.
 */

namespace FairPreferentialChoiceAlgorithms.Models.Datasets
{
    public class MetricsDataset
    {
        public string? MetricsName { get; set; }
        public string? HeuristicName { get; set; }
        public string? AlgorithmName { get; set; }
        public string? InputDatasetType { get; set; }
        public string? InputDatasetName { get; set; }
        public int ReducedCoursesCount { get; set; } = 0;                   // Anzahl der wegrationalisierten Kurse
        // Distributive Metriken
        public double UnassignedStudentsCount { get; set; } = 0;            // Anzahl unzugeteilter Schüler
        public double NormalizedAveragePreferenceRank { get; set; } = 0;    // Durchschnittlicher norm. Präferenzrang der Schüler
        public int FirstChoiceCount { get; set; } = 0;                      // X Schüler erhielten ihre Erstwahl
        public double FirstChoicePercentage { get; set; } = 0;              // X% der Teilnehmer erhielten ihre Erstwahl
        public int LastChoiceCount { get; set; } = 0;                       // X Schüler erhielten ihre Letztwahl
        public double LastChoicePercentage { get; set; } = 0;               // X% der Schüler erhielten ihre Letztwahl
        // Verbesserungsmetriken
        public List<int> JealousStudents { get; set; }                      // Liste der Schüler, die neidisch sind
        public int JealousStudentsCount { get; set; } = 0;                  // X Schüler sind neidisch
        public double JealousPercentage { get; set; } = 0;                  // X% der Schüler sind neidisch auf einen anderen
        public double NormalizedJealousy { get; set; } = 0;                 // Durchschnittlicher norm. Neidstärke der Schüler
        public List<(int, int)> TwoWaySwapPairs { get; set; }               // Liste der Paare, die tauschen würden
        public int TwoWaySwapPairsCount { get; set; } = 0;                  // X Paare würden gerne wollen
        public double TwoWaySwapPairsPercentage { get; set; } = 0;          // X% der Schüler würden gerne tauschen


        public MetricsDataset(AssignmentDataset assignmentDataset)
        {
            AssignmentDataset AssignmentDataset = assignmentDataset;
            MetricsName = "SINGLE";
            HeuristicName = AssignmentDataset.HeuristicName;
            AlgorithmName = AssignmentDataset.AlgorithmName;
            InputDatasetType = AssignmentDataset.InputDatasetType;
            InputDatasetName = AssignmentDataset.InputDatasetName;
            ReducedCoursesCount = AssignmentDataset.DeletedCoursesCount;
            // Distributive Metriken
            UnassignedStudentsCount = AssignmentDataset.UnassignedStudentsCount;
            var rankStats = CalculateAssignmentRankStats(AssignmentDataset);
            NormalizedAveragePreferenceRank = rankStats.AverageNormalizedRank;
            FirstChoiceCount = rankStats.FirstChoiceCount;
            FirstChoicePercentage = rankStats.FirstChoicePercentage;
            LastChoiceCount = rankStats.LastChoiceCount;
            LastChoicePercentage = rankStats.LastChoicePercentage;
            // Verbesserungsmetriken
            var envyStats = CalculateAssignmentEnvyStats(AssignmentDataset);
            JealousStudents = envyStats.JealousStudents;
            JealousStudentsCount = envyStats.JealousStudents.Count;
            JealousPercentage = envyStats.JealousPercentage;
            NormalizedJealousy = envyStats.NormalizedJealousy;
            TwoWaySwapPairs = envyStats.possibleSwapPairs;
            TwoWaySwapPairsCount = envyStats.possibleSwapPairs.Count;
            TwoWaySwapPairsPercentage = envyStats.possibleSwapPairsPercentage;
        }

        /// <summary>
        /// Behelfsmäßiger Konstruktor für das Aufsummieren von Statistiken<br/><br/>
        /// TODO: Auf Anzeigeseiten lieber Liste aller Metriken nach Begriffen filtern und Stats zusammenrechnen,<br/>
        /// dann könnte man auch anzeigen, wieviele Datensets man durchgegangen ist!
        /// </summary>
        public MetricsDataset(string heuristicName, string algorithmName, List<MetricsDataset> metricsDatasets)
        {
            MetricsName = $"{metricsDatasets.Count}";
            HeuristicName = heuristicName;
            AlgorithmName = algorithmName;
            InputDatasetType = "MULT";
            InputDatasetName = "MULT";
            ReducedCoursesCount = metricsDatasets.Sum(d => d.ReducedCoursesCount);
            // Distributive Metriken
            UnassignedStudentsCount = metricsDatasets.Sum(d => d.UnassignedStudentsCount);
            NormalizedAveragePreferenceRank = metricsDatasets.Average(d => d.NormalizedAveragePreferenceRank);
            FirstChoiceCount = metricsDatasets.Sum(d => d.FirstChoiceCount);
            FirstChoicePercentage = metricsDatasets.Average(d => d.FirstChoicePercentage);
            LastChoiceCount = metricsDatasets.Sum(d => d.LastChoiceCount);
            LastChoicePercentage = metricsDatasets.Average(d => d.LastChoicePercentage);
            TwoWaySwapPairsCount = metricsDatasets.Sum(d => d.TwoWaySwapPairsCount);
            TwoWaySwapPairsPercentage = metricsDatasets.Average(d => d.TwoWaySwapPairsPercentage);
            // Verbesserungsmetriken
            JealousStudentsCount = metricsDatasets.Sum(d => d.JealousStudentsCount);
            JealousPercentage = metricsDatasets.Average(d => d.JealousPercentage);
            NormalizedJealousy = metricsDatasets.Average(d => d.NormalizedJealousy);
            TwoWaySwapPairsCount = metricsDatasets.Sum(d => d.TwoWaySwapPairsCount);
            TwoWaySwapPairsPercentage = metricsDatasets.Average(d => d.TwoWaySwapPairsPercentage);
        }

        /// <summary>
        /// Berechnet den durchschnittlichen normierter Präferenzrang über alle Schüler.
        /// Der Wert liegt zwischen 0 (alle Erstwunsch erhalten) und 1 (alle Letztwunsch oder garnichts erhalten).
        /// Ermittelt auch, wie viele Schüler ihren Erstwunsch oder Letztwunsch erhalten haben,
        /// und berechnet den prozentualen Anteil beider Gruppen an der Gesamtschüleranzahl.
        /// </summary>
        /// <returns>
        /// Ein Tupel mit:<br/>
        /// - Durchschnittlichem normalisiertem Präferenzrang (0-1)<br/>
        /// - Anzahl der Schüler mit Erstwunschzuteilung (X-0)<br/>
        /// - Prozentualem Anteil der Erstwünsche (100-0)<br/>
        /// - Anzahl der Schüler mit Letztwunschzuteilung (0-X)<br/>
        /// - Prozentualem Anteil der Letztwünsche (0-100)
        /// </returns>
        public (double AverageNormalizedRank, int FirstChoiceCount,
                double FirstChoicePercentage, int LastChoiceCount,
                double LastChoicePercentage) CalculateAssignmentRankStats(AssignmentDataset assignmentDataset)
        {
            AssignmentDataset Data = assignmentDataset;

            // Summe aller normierten Ränge (0.0 = alle Erstwunsch erhalten, 1.0 = alle Letztwunsch erhalten)
            double totalNormalizedRank = 0.0;
            // Anzahl der Schüler, die in die Berechnung einfließen, da unzugewiesene Schüler nicht beachtet werden
            int totalStudents = 0;
            // Durchschnitt der Normierten Ränge
            double avgNormalizedRank = 0.0;
            // Anzahl der Schüler, die Erstwunsch erhalten haben
            int firstChoiceCount = 0;
            // Anteil an Schülern mit Erstwunsch 
            double firstChoicePercent = 0.0;
            // Anzahl der Schüler, die Letztwunsch erhalten haben
            int lastChoiceCount = 0;
            // Prozentuale Berechnung: Anteil an Schülern mit Letztwunsch
            double lastChoicePercent = 0.0;

            // Schleife über alle Schüler
            foreach (var student in Data.Students)
            {
                // Maximalen Präferenzrang ermitteln
                var Preferences = student.Preferences;
                int maxRank = Preferences.Count - 1;

                // Statistik-Failsafe: Wenn Schüler keine Präferenzen hat, zählt er nicht
                if (maxRank < 0)
                {
                    continue;
                }

                // Wenn Schüler keinen Kurs hat oder dieser nicht auf seinen Präferenzen steht, hat er den Letztwunsch
                if (!student.AssignedCourse.HasValue || Preferences.IndexOf(student.AssignedCourse.Value) == -1)
                {
                    lastChoiceCount++;
                    totalStudents++;
                    totalNormalizedRank += 1;
                    continue;
                }

                // Rang des zugewiesenen Kurses in der Präferenzliste holen
                var assignedCourse = student.AssignedCourse.Value;
                int rank = Preferences.IndexOf(assignedCourse);

                // Wenn zugewiesener Kurs an erster Stelle steht, hat der Schüler seinen Erstwunsch erhalten
                if (rank == 0 || maxRank == 0)
                {
                    firstChoiceCount++;
                    totalStudents++;
                    continue;
                }

                // Normierter Rang: 0 (Erstwunsch) bis 1 (Letztwunsch)
                // Wichtig, da jeder Schüler Zugang zu unterschiedlich vielen Kursen haben könnte!
                double normalizedRank = (double)rank / maxRank;
                if (normalizedRank == 1) {
                    lastChoiceCount++;
                }

                // Auf Gesamtränge draufzählen
                totalNormalizedRank += normalizedRank;
                totalStudents++;
            }

            // Failsafe, um nicht durch 0 zu teilen!
            if (totalStudents > 0)
            {
                firstChoicePercent = (double)firstChoiceCount / totalStudents * 100.0;
                lastChoicePercent = (double)lastChoiceCount / totalStudents * 100.0;
                avgNormalizedRank = totalNormalizedRank / totalStudents;
            }

            return (avgNormalizedRank, firstChoiceCount, firstChoicePercent, lastChoiceCount, lastChoicePercent);
        }



        /// <summary>
        /// Ermittelt die Metriken im Bezug auf Neid:<br/>
        /// Wieviele Schüler Neid empfinden, wie stark der empfundene Neid ist, 
        /// wer von einem beiderseitigem Tausch profitieren würde und jeweils die Anteile an der Gesamtmenge.
        /// </summary>
        /// <returns>
        /// Ein Tupel mit:<br/>
        /// - Liste der Schüler mit Neidempfinden<br/>
        /// - Prozentualem Anteil der Neidempfindenden (0-100)<br/>
        /// - Durchschnittlicher normalisierter Neidstärke (0-1)<br/>
        /// - Anzahl der Schüler mit Letztwunschzuteilung (0-X)<br/>
        /// - Liste der Schüler, die von beiderseitigem Tausch profitieren würden<br/>
        /// - Prozentualer Anteil dieser Paare an der Gesamtmenge (0–100)
        /// </returns>

                public (List<int> JealousStudents,
                        double JealousPercentage,
                        double NormalizedJealousy,
                        List<(int A, int B)> possibleSwapPairs,
                        double possibleSwapPairsPercentage)
                CalculateAssignmentEnvyStats(AssignmentDataset assignmentDataset)
        {

            var students = assignmentDataset.Students;
            var courseCount = (assignmentDataset.Courses.Count - 1);

            // Schüler, die neidisch auf andere Schüler sind
            var jealousStudents = new HashSet<int>();
            // Summe aller normierten Neidwerte
            double totalJealousy = 0.0;
            // Summe Fälle von Neid
            int jealousyCounter = 0;
            // Mögliche gegenseitige Tauschpaare (beide profitieren)
            var possibleSwapPairs = new List<(int, int)>();
            // Einzelne Schüler, die in Tauschpaaren erwähnt werden
            var possibleSwapPairsStudents = new HashSet<int>();

            // Äußere Schleife über alle Schüler A
            for (int i = 0; i < students.Count; i++)
            {
                // Schüler A
                var a = students[i];

                // Failsafe: Wenn Schüler A keinen Kurs oder keine Präferenzen hat, zählt er nicht
                if (!a.AssignedCourse.HasValue || a.Preferences == null)
                        {
                    continue;
                        }

                // Rang des aktuellen Kurses von A in der Präferenzliste
                var aCourse = a.AssignedCourse.Value;
                var aPreferences = a.Preferences;
                int aRank = aPreferences.IndexOf(aCourse);

                // Wenn Schüler A Kurs nicht auf der Liste hatte, bekommt er den schlechtesten Rang
                if (aRank == -1)
                        {
                    aRank = courseCount;
                        }

                // Innere Schleife über alle Schüler B (Durch j > i wird jedes Paar nur einmal betrachtet)
                for (int j = i + 1; j < students.Count; j++)
                {
                    // Schüler B
                    var b = students[j];

                    // Failsafe: Wenn Schüler B keinen Kurs hat zählt er nicht
                    if (!b.AssignedCourse.HasValue || b.Preferences == null)
                            {
                        continue;
                            }

                    // Rang des aktuellen Kurses von B in der Präferenzliste
                    var bCourse = b.AssignedCourse.Value;
                    var bPreferences = b.Preferences;
                    int bRank = bPreferences.IndexOf(bCourse);

                    // Wenn Schüler B den Kurs nicht auf der Liste hatte, bekommt der Kurs den schlechtesten Rang
                    if (bRank == -1)
                            {
                        bRank = courseCount;
                            }

                    // Wie gut finden A und B die Kurse voneinander?
                    int aCompareRank = aPreferences.IndexOf(bCourse);
                    int bCompareRank = bPreferences.IndexOf(aCourse);

                    // Wenn sie den jeweils anderen Kurs nicht gewünscht haben,
                    // ist dieser aus ihrer Sicht maximal schlecht zu bewerten
                    if (aCompareRank == -1) aCompareRank = courseCount;
                    if (bCompareRank == -1) bCompareRank = courseCount;

                    // Ist A neidisch auf B? (mit unsauberem Failsafe)
                    if (aCompareRank < aRank)
                    {
                        jealousStudents.Add(a.Id);
                        totalJealousy += (aRank - aCompareRank) / (double)(aPreferences.Count - 1);
                        jealousyCounter++;
                    }
                    // Ist B neidisch auf A? (mit unsauberem Failsafe)
                    if (bCompareRank < bRank)
                    {
                        jealousStudents.Add(b.Id);
                        totalJealousy += (bRank - bCompareRank) / (double)(bPreferences.Count - 1);
                        jealousyCounter++;
                    }

                    // Werden beide durch einen Tausch bessergestellt?
                    if (aCompareRank < aRank && bCompareRank < bRank)
                    {
                        possibleSwapPairs.Add((a.Id, b.Id));
                        possibleSwapPairsStudents.Add(a.Id);
                        possibleSwapPairsStudents.Add(b.Id);
                    }
                }
            }

            // Wieviele Schüler sind einem Kurs zugewiesen?
            int assignedStudentsCount = students.Count(s => s.AssignedCourse.HasValue);
            // Anteil neidischer Schüler an der Gesamtmenge (Failsafe für Nullteilung)
                    double jealousStudentsPercentage = assignedStudentsCount > 0 ? (double)jealousStudents.Count / assignedStudentsCount * 100.0 : 0.0;
            // Wie stark ist der Neid der Gesamtmenge? (1 = extrem, 0 = garnicht) (Failsafe für Nullteilung)
                    double normalizedJealousy = jealousyCounter > 0 ? totalJealousy / jealousyCounter : 0.0;
            // Anteil möglicher Tauschpaare an der Gesamtmenge (Failsafe für Nullteilung)
                    double possibleSwapPairsPercentage = assignedStudentsCount > 0 ? (double)possibleSwapPairsStudents.Count / assignedStudentsCount * 100.0 : 0.0;
                    // Ergebnis-Tupel zurückgeben: Neidische Schüler, Anteil der neidischen Schüler, Neidwert, mögliche Paare für einen beiderseitig vorteilhaften Tausch
                    return (jealousStudents.ToList(), jealousStudentsPercentage, normalizedJealousy, possibleSwapPairs, possibleSwapPairsPercentage);
        }
    }
}
