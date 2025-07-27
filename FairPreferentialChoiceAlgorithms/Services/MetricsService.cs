using FairPreferentialChoiceAlgorithms.Models.Datasets;
using System;

namespace FairPreferentialChoiceAlgorithms.Services
{
    public class MetricsService
    {
        private readonly AssignmentService _assignmentService;

        // Listen nach Daten
        public List<MetricsDataset> Metrics { get; set; } = new List<MetricsDataset>();
        public List<MetricsDataset> TotalMetrics { get; set; } = new List<MetricsDataset>();

        public MetricsService(AssignmentService assignmentService)
        {
            _assignmentService = assignmentService;

            foreach (var dataset in assignmentService.Assignments)
            {
                // Nur Metriken generieren, wenn auch eine erfolgreiche Verteilung stattgefunden hat.
                if(dataset.UnassignedStudentsCount == 0)
                {
                    Metrics.Add(new MetricsDataset(dataset));
                }
            }

            //assignmentService.Assignments.GET X
            
        }

        public static double CalculateEnvyIndex(AssignmentDataset dataset)
        {

            /*
            var students = dataset.InputDataset.Preferences;
            var assignments = dataset.Assignments;

            int total = students.Count;
            int enviousCount = 0;

            foreach (var a in students)
            {
                var aAssigned = assignments[a.Id];

                foreach (var b in students)
                {
                    if (a.Id == b.Id) continue;

                    var bAssigned = assignments[b.Id];
                    if (bAssigned == null) continue; // B hat nichts, nichts zu beneiden

                    // Fall 1: A hat keinen Kurs → neidisch auf alles, was in Präferenzliste steht
                    if (aAssigned == null && a.Preferences.Contains(bAssigned.Value))
                    {
                        enviousCount++;
                        break;
                    }

                    // Fall 2: A hat Kurs, bevorzugt aber B's Kurs
                    if (aAssigned != null &&
                        Prefers(a.Preferences, bAssigned.Value, aAssigned.Value))
                    {
                        enviousCount++;
                        break;
                    }
                }
            }

            return total == 0 ? 0 : (double)enviousCount / total;
            */
            return 2;
        }

        /// <summary>
        /// Gibt true zurück, wenn 'preferred' vor 'actual' in der Präferenzliste steht.
        /// </summary>
        private static bool Prefers(List<int> preferences, int preferred, int actual)
        {
            int preferredIndex = preferences.IndexOf(preferred);
            int actualIndex = preferences.IndexOf(actual);

            if (preferredIndex == -1) return false;
            if (actualIndex == -1) return true;

            return preferredIndex < actualIndex;
        }
    }
}
