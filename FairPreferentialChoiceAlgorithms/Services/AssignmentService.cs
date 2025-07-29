using FairPreferentialChoiceAlgorithms.Models.Datasets;
using FairPreferentialChoiceAlgorithms.Services.Algorithms;
using FairPreferentialChoiceAlgorithms.Services.Heuristics;
using System;

namespace FairPreferentialChoiceAlgorithms.Services
{
    public class AssignmentService
    {
        private readonly Random _random;
        private readonly InputDataService _inputDataService;

        public readonly List<IAssignmentHeuristic> heuristics;
        public readonly List<IAssignmentAlgorithm> algorithms;

        public List<AssignmentDataset> Assignments { get; set; } = new List<AssignmentDataset>();

        public AssignmentService(Random random, InputDataService inputDataService)
        {
            _random = random;
            _inputDataService = inputDataService;

            // Liste aller Heuristiken, die verwendet werden sollen
            heuristics = new List<IAssignmentHeuristic>
            {
                new HeuristicNone(),
                new HeuristicOMX(1),
                new HeuristicOMX(2),
                new HeuristicOMX(3)

            };

            // Liste aller Algorithmen, die verwendet werden sollen
            algorithms = new List<IAssignmentAlgorithm>
            {
                new AlgorithmBOS(_random),
                new AlgorithmDAC(_random),
                new AlgorithmRSD(_random)
                //new AlgorithmTTC(_random)
            };

            foreach (var heuristic in heuristics)
            {
                foreach (var algorithm in algorithms)
                {
                    foreach (var dataset in _inputDataService.Datasets)
                    {
                        var result = heuristic.CreateAssignment(dataset, algorithm);
                        result.InputDatasetName = dataset.Name;
                        result.AlgorithmName = algorithm.Name;
                        result.HeuristicName = heuristic.Name;

                        Assignments.Add(result);
                    }
                }
            }

            // Reale, historische Zuteilungen
            AddRealAssignments();

        }

        public void AddRealAssignments()
        {
            AssignmentDataset mgs23v = new AssignmentDataset(){
                HeuristicName = "OM1",
                AlgorithmName = "UNK",
                InputDatasetType = "HIST",
                InputDatasetName = "MGS-23-V",
                DeletedCoursesCount = 1,
                Courses = new List<(int Id, int? Capacity, int? Minimum)>
                {
                    (0, 30, 15),
                    (1, 30, 15),
                    (2, 30, 15),
                    (3, 30, 15),
                    (4, 30, 15)
                },
                Students = new List<(int Id, int? AssignedCourse, List<int> Preferences)>
                {
                    (0, 4, new(){4, 2, 3, 0}),
                    (1, 2, new(){0, 2, 4, 3}),
                    (2, 4, new(){4, 2, 3, 0}),
                    (3, 1, new(){2, 1, 3, 4}),
                    (4, 2, new(){0, 2, 4, 3}),
                    (5, 3, new(){3, 2, 4, 1}),
                    (6, 1, new(){1, 3, 2, 4}),
                    (7, 3, new(){2, 3, 4, 1}),
                    (8, 2, new(){2, 4, 3, 1}),
                    (9, 1, new(){2, 1, 3, 4}),
                    (10, 4, new(){4, 2, 1, 3}),
                    (11, 4, new(){4, 3, 2, 1}),
                    (12, 3, new(){2, 3, 4, 0}),
                    (13, 4, new(){4, 2, 3, 1}),
                    (14, 4, new(){4, 2, 3, 0}),
                    (15, 4, new(){4, 2, 3, 1}),
                    (16, 4, new(){4, 2, 1, 3}),
                    (17, 1, new(){4, 1, 2, 3}),
                    (18, 3, new(){2, 3, 0, 4}),
                    (19, 2, new(){2, 4, 1, 3}),
                    (20, 4, new(){4, 2, 3, 0}),
                    (21, 3, new(){3, 2, 0, 4}),
                    (22, 4, new(){4, 2, 3, 0}),
                    (23, 2, new(){2, 4, 3, 0}),
                    (24, 4, new(){4, 2, 3, 0}),
                    (25, 3, new(){2, 3, 4, 0}),
                    (26, 1, new(){2, 1, 3, 4}),
                    (27, 2, new(){2, 4, 3, 1}),
                    (28, 4, new(){4, 2, 3, 1}),
                    (29, 1, new(){1, 2, 4, 3}),
                    (30, 2, new(){2, 4, 1, 3}),
                    (31, 3, new(){3, 1, 2, 4}),
                    (32, 4, new(){4, 2, 3, 0}),
                    (33, 3, new(){3, 0, 4, 2}),
                    (34, 1, new(){1, 2, 4, 3}),
                    (35, 3, new(){2, 3, 4, 0}),
                    (36, 2, new(){2, 4, 3, 0}),
                    (37, 2, new(){2, 4, 3, 0}),
                    (38, 2, new(){2, 4, 1, 3}),
                    (39, 3, new(){3, 4, 0, 2}),
                    (40, 3, new(){0, 3, 2, 4}),
                    (41, 2, new(){2, 4, 3, 0}),
                    (42, 3, new(){2, 3, 4, 0}),
                    (43, 4, new(){4, 0, 3, 2}),
                    (44, 2, new(){2, 4, 3, 1}),
                    (45, 2, new(){2, 4, 3, 0}),
                    (46, 4, new(){4, 2, 3, 0}),
                    (47, 3, new(){3, 2, 4, 0}),
                    (48, 4, new(){4, 0, 3, 2}),
                    (49, 2, new(){2, 4, 3, 1}),
                    (50, 3, new(){3, 4, 2, 1}),
                    (51, 2, new(){2, 4, 3, 1}),
                    (52, 1, new(){2, 1, 3, 4}),
                    (53, 4, new(){4, 2, 3, 0}),
                    (54, 4, new(){4, 2, 3, 0}),
                    (55, 3, new(){2, 3, 1, 4}),
                    (56, 4, new(){4, 3, 1, 2}),
                    (57, 2, new(){2, 4, 3, 1}),
                    (58, 4, new(){4, 3, 2, 0}),
                    (59, 3, new(){2, 3, 1, 4}),
                    (60, 2, new(){2, 4, 3, 1}),
                    (61, 4, new(){4, 2, 3, 0}),
                    (62, 4, new(){4, 2, 1, 3}),
                    (63, 2, new(){2, 4, 3, 1}),
                    (64, 4, new(){4, 2, 3, 0}),
                    (65, 4, new(){4, 2, 3, 1}),
                    (66, 2, new(){2, 4, 3, 1}),
                    (67, 2, new(){2, 4, 1, 3}),
                    (68, 4, new(){0, 4, 3, 2}),
                    (69, 2, new(){2, 3, 4, 0}),
                    (70, 2, new(){2, 4, 3, 1}),
                    (71, 4, new(){4, 2, 3, 0}),
                    (72, 2, new(){2, 4, 3, 0}),
                    (73, 2, new(){3, 2, 4, 1})
                }
            };
            Assignments.Add(mgs23v);

            AssignmentDataset mgs24v = new AssignmentDataset()
            {
                HeuristicName = "OM3",
                AlgorithmName = "UNK",
                InputDatasetType = "HIST",
                InputDatasetName = "MGS-24-V",
                DeletedCoursesCount = 2,
                Courses = new List<(int Id, int? Capacity, int? Minimum)>
                {
                    (0, 28, 15),
                    (1, 28, 15),
                    (2, 56, 15),
                    (3, 28, 15),
                    (4, 28, 15)
                },
                Students = new List<(int Id, int? AssignedCourse, List<int> Preferences)>
                {
                    (0, 2, new(){2, 3, 4, 0}),
                    (1, 2, new(){2, 4, 3, 1}),
                    (2, 3, new(){3, 2, 4, 1}),
                    (3, 2, new(){2, 3, 4, 0}),
                    (4, 2, new(){2, 4, 3, 1}),
                    (5, 3, new(){3, 2, 4, 1}),
                    (6, 2, new(){2, 1, 3, 4}),
                    (7, 2, new(){2, 3, 4, 1}),
                    (8, 3, new(){3, 4, 2, 1}),
                    (9, 3, new(){2, 3, 4, 0}),
                    (10, 3, new(){3, 2, 1, 4}),
                    (11, 3, new(){3, 2, 4, 1}),
                    (12, 2, new(){2, 3, 4, 1}),
                    (13, 3, new(){3, 4, 2, 0}),
                    (14, 2, new(){2, 3, 4, 1}),
                    (15, 2, new(){2, 3, 4, 1}),
                    (16, 2, new(){2, 4, 3, 1}),
                    (17, 3, new(){4, 3, 2, 1}),
                    (18, 2, new(){2, 4, 3, 1}),
                    (19, 3, new(){3, 2, 4, 0}),
                    (20, 3, new(){3, 4, 1, 2}),
                    (21, 2, new(){2, 3, 0, 4}),
                    (22, 2, new(){2, 4, 3, 1}),
                    (23, 2, new(){2, 3, 4, 1}),
                    (24, 3, new(){2, 4, 3, 0}),
                    (25, 3, new(){3, 4, 2, 0}),
                    (26, 3, new(){3, 2, 0, 4}),
                    (27, 3, new(){3, 2, 4, 0}),
                    (28, 3, new(){0, 3, 4, 2}),
                    (29, 3, new(){3, 2, 4, 1}),
                    (30, 2, new(){2, 3, 4, 0}),
                    (31, 2, new(){2, 3, 4, 0}),
                    (32, 3, new(){1, 4, 3, 2}),
                    (33, 3, new(){3, 2, 4, 1}),
                    (34, 2, new(){2, 3, 4, 0}),
                    (35, 3, new(){3, 4, 2, 1}),
                    (36, 2, new(){2, 4, 3, 1}),
                    (37, 2, new(){2, 3, 1, 4}),
                    (38, 2, new(){2, 1, 3, 4}),
                    (39, 2, new(){0, 2, 3, 4}),
                    (40, 2, new(){2, 4, 3, 0}),
                    (41, 3, new(){2, 4, 3, 1}),
                    (42, 2, new(){2, 3, 4, 0}),
                    (43, 2, new(){2, 3, 4, 0}),
                    (44, 2, new(){2, 3, 4, 0}),
                    (45, 2, new(){2, 3, 4, 1}),
                    (46, 2, new(){2, 3, 0, 4}),
                    (47, 2, new(){2, 4, 3, 1}),
                    (48, 3, new(){2, 3, 4, 1}),
                    (49, 2, new(){2, 3, 4, 0}),
                    (50, 2, new(){2, 3, 1, 4}),
                    (51, 2, new(){2, 4, 3, 1}),
                    (52, 2, new(){2, 3, 4, 1}),
                    (53, 2, new(){2, 3, 4, 0}),
                    (54, 2, new(){2, 4, 3, 0}),
                    (55, 2, new(){2, 3, 4, 1}),
                    (56, 2, new(){2, 4, 3, 0}),
                    (57, 2, new(){2, 3, 4, 1}),
                    (58, 2, new(){2, 3, 4, 0}),
                    (59, 2, new(){2, 4, 3, 0}),
                    (60, 2, new(){4, 2, 3, 1}),
                    (61, 3, new(){3, 2, 4, 1}),
                    (62, 2, new(){2, 4, 3, 1}),
                    (63, 3, new(){3, 2, 4, 0}),
                    (64, 2, new(){2, 3, 4, 0}),
                    (65, 2, new(){2, 3, 4, 0})
                }
            };
            Assignments.Add(mgs24v);

            AssignmentDataset mgs25v = new AssignmentDataset()
            {
                HeuristicName = "STD",
                AlgorithmName = "UNK",
                InputDatasetType = "HIST",
                InputDatasetName = "MGS-25-V",
                DeletedCoursesCount = 0,
                Courses = new List<(int Id, int? Capacity, int? Minimum)>
                {
                    (0, 28, 15),
                    (1, 28, 15),
                    (2, 28, 15),
                    (3, 28, 15)
                },
                Students = new List<(int Id, int? AssignedCourse, List<int> Preferences)>
                {
                    (0, 3, new(){0, 3, 1, 2}),
                    (1, 0, new(){0, 3, 1, 2}),
                    (2, 3, new(){3, 2, 1, 0}),
                    (3, 2, new(){2, 3, 1, 0}),
                    (4, 2, new(){2, 3, 1, 0}),
                    (5, 1, new(){0, 1, 3, 2}),
                    (6, 0, new(){0, 3, 1, 2}),
                    (7, 1, new(){1, 0, 3, 2}),
                    (8, 1, new(){0, 1, 3, 2}),
                    (9, 3, new(){0, 3, 2, 1}),
                    (10, 1, new(){0, 1, 3, 2}),
                    (11, 0, new(){0, 1, 3, 2}),
                    (12, 1, new(){1, 3, 2, 0}),
                    (13, 3, new(){0, 3, 1, 2}),
                    (14, 1, new(){1, 0, 2, 3}),
                    (15, 1, new(){1, 0, 2, 3}),
                    (16, 3, new(){3, 1, 0, 2}),
                    (17, 3, new(){2, 3, 0, 1}),
                    (18, 1, new(){1, 0, 2, 3}),
                    (19, 1, new(){0, 1, 3, 2}),
                    (20, 3, new(){3, 0, 2, 1}),
                    (21, 2, new(){0, 2, 3, 1}),
                    (22, 2, new(){2, 3, 0, 1}),
                    (23, 0, new(){0, 2, 3, 1}),
                    (24, 0, new(){0, 1, 3, 2}),
                    (25, 2, new(){2, 3, 1, 0}),
                    (26, 2, new(){2, 3, 0, 1}),
                    (27, 1, new(){1, 2, 0, 3}),
                    (28, 3, new(){3, 0, 1, 2}),
                    (29, 1, new(){1, 0, 3, 2}),
                    (30, 2, new(){2, 3, 0, 1}),
                    (31, 0, new(){0, 1, 3, 2}),
                    (32, 3, new(){0, 3, 2, 1}),
                    (33, 0, new(){0, 1, 3, 2}),
                    (34, 2, new(){0, 2, 3, 1}),
                    (35, 1, new(){1, 0, 2, 3}),
                    (36, 1, new(){1, 0, 2, 3}),
                    (37, 3, new(){3, 2, 0, 1}),
                    (38, 2, new(){2, 1, 3, 0}),
                    (39, 0, new(){0, 2, 3, 1}),
                    (40, 1, new(){1, 2, 3, 0}),
                    (41, 0, new(){0, 1, 3, 2}),
                    (42, 2, new(){2, 3, 1, 0}),
                    (43, 3, new(){3, 2, 0, 1}),
                    (44, 0, new(){0, 3, 1, 2}),
                    (45, 2, new(){2, 0, 1, 3}),
                    (46, 0, new(){0, 1, 2, 3}),
                    (47, 0, new(){0, 1, 3, 2}),
                    (48, 0, new(){0, 1, 3, 2}),
                    (49, 1, new(){1, 3, 2, 0}),
                    (50, 1, new(){1, 0, 2, 3}),
                    (51, 1, new(){1, 2, 0, 3}),
                    (52, 1, new(){0, 1, 2, 3}),
                    (53, 2, new(){2, 0, 3, 1}),
                    (54, 0, new(){1, 3, 0, 2}),
                    (55, 3, new(){3, 2, 0, 1}),
                    (56, 0, new(){0, 1, 2, 3}),
                    (57, 2, new(){2, 3, 0, 1}),
                    (58, 2, new(){2, 3, 1, 0}),
                    (59, 3, new(){3, 2, 0, 1}),
                    (60, 3, new(){3, 2, 0, 1}),
                    (61, 3, new(){3, 2, 1, 0}),
                    (62, 0, new(){0, 1, 3, 2}),
                    (63, 3, new(){0, 3, 2, 1}),
                    (64, 2, new(){2, 3, 1, 0}),
                    (65, 2, new(){2, 3, 0, 1}),
                    (66, 1, new(){0, 2, 3, 1}),
                    (67, 3, new(){0, 3, 1, 2}),
                    (68, 2, new(){0, 2, 1, 3}),
                    (69, 0, new(){0, 1, 3, 2}),
                    (70, 0, new(){0, 1, 2, 3}),
                    (71, 1, new(){1, 0, 3, 2}),
                    (72, 2, new(){2, 0, 3, 1}),
                    (73, 3, new(){0, 3, 1, 2}),
                    (74, 2, new(){1, 2, 0, 3}),
                    (75, 0, new(){0, 3, 1, 2}),
                    (76, 3, new(){3, 2, 0, 1}),
                    (77, 3, new(){3, 2, 0, 1}),
                    (78, 3, new(){3, 1, 0, 2}),
                    (79, 2, new(){2, 3, 0, 1}),
                    (80, 1, new(){1, 0, 3, 2}),
                    (81, 2, new(){2, 3, 0, 1}),
                    (82, 3, new(){0, 3, 1, 2}),
                    (83, 3, new(){3, 0, 2, 1}),
                    (84, 0, new(){0, 1, 1, 2}),
                    (85, 2, new(){0, 2, 3, 1}),
                    (86, 1, new(){1, 2, 0, 3}),
                    (87, 2, new(){2, 1, 0, 3}),
                    (88, 0, new(){0, 2, 3, 1}),
                    (89, 1, new(){1, 3, 2, 0}),
                    (90, 3, new(){3, 2, 1, 0}),
                    (91, 2, new(){2, 0, 1, 3}),
                    (92, 0, new(){0, 3, 1, 2}),
                    (93, 2, new(){2, 0, 3, 1}),
                    (94, 3, new(){3, 1, 0, 2}),
                    (95, 0, new(){0, 1, 3, 2}),
                    (96, 3, new(){0, 3, 1, 2}),
                    (97, 1, new(){1, 3, 2, 0}),
                    (98, 0, new(){0, 1, 2, 3})
                }
            };
            Assignments.Add(mgs25v);

            /*
            // Simples Testszenario für die Neidberechnungen
            AssignmentDataset neid = new AssignmentDataset()
            {
                HeuristicName = "STD",
                AlgorithmName = "UNK",
                InputDatasetType = "WRST",
                InputDatasetName = "NEID",
                DeletedCoursesCount = 0,
                Courses = new List<(int Id, int? Capacity, int? Minimum)>
                {
                    (0, 28, 15),
                    (1, 28, 15),
                    (2, 28, 15),

                },
                Students = new List<(int Id, int? AssignedCourse, List<int> Preferences)>
                {
                    (0, 0, new(){0, 1, 2}),
                    (1, 0, new(){0, 2, 1}),
                    (2, 0, new(){1, 2, 0}),
                    (3, 0, new(){1, 0, 2}),
                    (4, 2, new(){2, 1, 0}),
                    (5, 0, new(){2, 0, 0}),
                    (6, 1, new(){0, 1, 2}),
                    (7, 1, new(){0, 2, 1}),
                    (8, 1, new(){1, 0, 2}),
                    (9, 1, new(){1, 2, 0}),
                    (10, 2, new(){2, 1, 0}),
                    (11, 1, new(){2, 0, 1}),
                }
            };
            Assignments.Add(neid);
            */
        }
    }
}
