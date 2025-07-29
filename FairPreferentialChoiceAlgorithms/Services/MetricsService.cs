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
        }
    }
}
