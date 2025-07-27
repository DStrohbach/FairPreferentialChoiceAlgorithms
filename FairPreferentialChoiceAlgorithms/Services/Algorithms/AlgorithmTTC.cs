using FairPreferentialChoiceAlgorithms.Models;
using System;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
    /// <summary>
    /// Top Trading Cycles: strategiesicher<br/>
    /// <para>Siehe <a href="https://en.wikipedia.org/wiki/Top_trading_cycle">TTC auf Wikipedia</a></para>
    /// <para>Siehe <a href="https://people.duke.edu/~aa88/articles/BostonAERPP.pdf">Paper mit Abläufen von Algorithmen</a></para>
    /// </summary>
    public class AlgorithmTTC : IAssignmentAlgorithm
    {
        private readonly Random _random;
        public string Name => "TTC";

        public AlgorithmTTC(Random random)
        {
            _random = random;
        }

        public void Run(List<Course> courses, List<Student> students)
        {
            // TODO: Implementieren...
        }
    }
}
