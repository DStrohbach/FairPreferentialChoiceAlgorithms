using FairPreferentialChoiceAlgorithms.Models;
using FairPreferentialChoiceAlgorithms.Models.Datasets;
using FairPreferentialChoiceAlgorithms.Services.Algorithms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;


namespace FairPreferentialChoiceAlgorithms.Services
{
    public class InputDataService
    {
        private readonly Random _random;
        public List<InputDataset> Datasets { get; set; } = new List<InputDataset>();

        public List<InputDataset> HistoricalInputs { get; set; } = new List<InputDataset>();
        public List<InputDataset> BestCaseInputs { get; set; } = new List<InputDataset>();
        public List<InputDataset> WorstCaseInputs { get; set; } = new List<InputDataset>();
        public List<InputDataset> RandomInputs { get; set; } = new List<InputDataset>();

        public InputDataService(Random random)
        {
            _random = random;

            CreateHistoricalInputs();
            CreateBestCaseInputs();
            CreateWorstCaseInputs();
            CreateRandomInputs();
        }

        public void CreateHistoricalInputs()
        {
            AddDataFromDirectoryToList("Historical", Datasets);
        }

        public void CreateBestCaseInputs()
        {
            AddDataFromDirectoryToList("BestCase", Datasets);
        }

        public void CreateWorstCaseInputs()
        {
            AddDataFromDirectoryToList("WorstCase", Datasets);

            // 5 Kurse je 20 Plätze, 100 gleiche Präferenzlisten
            InputDataset SameList5K100 = new()
            {
                Type = "WRST",
                Name = "WC-100-5K",
                Courses = new List<CourseData>
                {
                    new(20, null),
                    new(20, null),
                    new(20, null),
                    new(20, null),
                    new(20, null)
                }
            };
            for (int i = 0; i < 100; i++)
            {
                SameList5K100.Preferences.Add((new() { 0, 1, 2, 3, 4 }));
            }
            Datasets.Add(SameList5K100);
            
        }

        public void CreateRandomInputs()
        {
            // 5 Kurse je 25 Plätze, Minimalbelegung 12, 100 zufällige Präferenzlisten mit bis zu -1 Präferenzen
            for (int i = 0; i < 300; i++)
            {
                InputDataset rndData = new()
                {
                    Type = "RAND",
                    Name = $"RND-5K-{i:D3}",
                    Courses = new List<CourseData>
                    {
                        new(25, 12),
                        new(25, 12),
                        new(25, 12),
                        new(25, 12),
                        new(25, 12)
                    }
                };
                var preferences = GenerateRandomPreferences(
                    numberOfStudents: 100,
                    numberOfCourses: rndData.Courses.Count,
                    minPreferencesPerStudent: rndData.Courses.Count - 1
                );
                rndData.Preferences = preferences;

                Datasets.Add(rndData);
            }

            // 4 Kurse je 30 Plätze, Minimalbelegung 10, 100 zufällige Präferenzlisten mit bis zu -1 Präferenzen
            for (int i = 0; i < 300; i++)
            {
                InputDataset rndData = new()
                {
                    Type = "RAND",
                    Name = $"RND-4K-{i:D3}",
                    Courses = new List<CourseData>
                    {
                        new(30, 10),
                        new(30, 10),
                        new(30, 10),
                        new(30, 10)
                    }
                };
                var preferences = GenerateRandomPreferences(
                    numberOfStudents: 100,
                    numberOfCourses: rndData.Courses.Count,
                    minPreferencesPerStudent: rndData.Courses.Count-1
                );
                rndData.Preferences = preferences;

                Datasets.Add(rndData);
            }

            // 3 Kurse je 30 Plätze, Minimalbelegung 15, 80 zufällige Präferenzlisten ohne Einschränkung
            for (int i = 0; i < 300; i++)
            {
                InputDataset rndData = new()
                {
                    Type = "RAND",
                    Name = $"RND-3K-{i:D3}",
                    Courses = new List<CourseData>
                    {
                        new(30, 15),
                        new(30, 15),
                        new(30, 15)
                    }
                };
                var preferences = GenerateRandomPreferences(
                    numberOfStudents: 80,
                    numberOfCourses: rndData.Courses.Count,
                    minPreferencesPerStudent: rndData.Courses.Count
                );
                rndData.Preferences = preferences;

                Datasets.Add(rndData);
            }
            
        }

        public  List<List<int>> GenerateRandomPreferences(int numberOfStudents, int numberOfCourses, int minPreferencesPerStudent)
        {
            // Failsafe
            if (minPreferencesPerStudent > numberOfCourses)
            {
                minPreferencesPerStudent = numberOfCourses;
            }

            List<List<int>> preferences = new();

            // Kurs-IDs von 0 bis numberOfCourses-1
            List<int> allCourses = Enumerable.Range(0, numberOfCourses).ToList();

            for (int i = 0; i < numberOfStudents; i++)
            {
                // Anzahl der Präferenzen für diesen Studenten: zwischen min und numberOfCourses
                int prefsCount = _random.Next(minPreferencesPerStudent, numberOfCourses + 1);

                // Kopiere und mische
                List<int> shuffledCourses = new(allCourses);
                shuffledCourses.Shuffle(_random);

                // Die ersten prefsCount auswählen
                preferences.Add(shuffledCourses.Take(prefsCount).ToList());
            }

            return preferences;
        }

        public void AddDataFromDirectoryToList(string directory, List<InputDataset> datasetList)
        {
            // Failsafe
            if (Directory.Exists($"Data/Inputs/{directory}")) {
                var dataDirectory = Path.Combine(AppContext.BaseDirectory, $"Data/Inputs/{directory}");
                foreach (var filePath in Directory.GetFiles(dataDirectory, "*.json"))
                {
                    var json = File.ReadAllText(filePath);
                    var dataset = JsonSerializer.Deserialize<InputDataset>(json);
                    if (dataset != null)
                    {
                        dataset.Name ??= Path.GetFileNameWithoutExtension(filePath); // Fallback auf Dateiname
                        datasetList.Add(dataset);
                    }
                }
            }
        }
    }
}
