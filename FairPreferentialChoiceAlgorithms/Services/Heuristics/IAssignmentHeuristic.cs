using FairPreferentialChoiceAlgorithms.Models;
using FairPreferentialChoiceAlgorithms.Models.Datasets;
using FairPreferentialChoiceAlgorithms.Services.Algorithms;

namespace FairPreferentialChoiceAlgorithms.Services.Heuristics
{
    public interface IAssignmentHeuristic
    {
        string Name { get; }
        AssignmentDataset CreateAssignment(InputDataset setup, IAssignmentAlgorithm algorithm, int deletedCourses = 0);
    }
}
