using System.Collections.Generic;

namespace FairPreferentialChoiceAlgorithms.Models.Datasets
{
    public class InputDataset
    {
        /// <summary>
        /// Kategorie des Datensatzes (historisch, bestcase, worstcase, ...).
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// Name des Datensatzes.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Liste der verfügbaren Kurse (Kapazität, Minimum).
        /// </summary>
        public List<CourseData> Courses { get; set; } = new();
        /// <summary>
        /// Schülerdaten mit Präferenzlisten (Kurs-IDs).
        /// </summary>
        public List<List<int>> Preferences { get; set; } = new();

        public InputDataset() {}

        public InputDataset(string type, string name, List<CourseData> courses, List<List<int>> preferences)
        {
            Type = type;
            Name = name;
            Courses = courses;
            Preferences = preferences;
        }
    }

    public class CourseData
    {
        public int? Capacity { get; set; }
        public int? Minimum { get; set; }

        public CourseData() {}

        public CourseData(int? capacity, int? minimum)
        {
            Capacity = capacity;
            Minimum = minimum;
        }
    }
}
