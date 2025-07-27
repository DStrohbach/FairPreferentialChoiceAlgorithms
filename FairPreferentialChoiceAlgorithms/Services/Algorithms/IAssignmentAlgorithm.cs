using FairPreferentialChoiceAlgorithms.Models;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
    public interface IAssignmentAlgorithm
    {
        string Name { get; }
        void Run(List<Course> courses, List<Student> students);
    }
}
