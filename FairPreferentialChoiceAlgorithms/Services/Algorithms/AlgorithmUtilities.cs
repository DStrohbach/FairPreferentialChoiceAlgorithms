using FairPreferentialChoiceAlgorithms.Models;

namespace FairPreferentialChoiceAlgorithms.Services.Algorithms
{
    public static class AlgorithmUtilities
    {

        /// <summary>
        /// Mischt eine Liste mit dem Fisher-Yates-Algorithmus:<br/>
        /// Geht die Liste von hinten nach vorne durch und vertauscht den aktuellen Eintrag zufällig mit einem verbleibenden oder sich selbst.<br/>
        /// Zeitkomplexität: O(n); Vorteil: Bessere Zufallsverteilung als OrderBy Random.
        /// <para>Siehe <a href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm">Fisher–Yates shuffle auf Wikipedia</a></para>
        /// </summary>
        /// <typeparam name="T">Typ der Listenelemente</typeparam>
        /// <param name="list">Die zu mischende Liste</param>
        /// <param name="rng">Instanz von Random</param>
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            // 1. Starte beim letzten Index der Liste, bewege dich zum Anfang der Liste
            for (int i = list.Count - 1; i > 0; i--)
            {
                // 2. Wähle einen zufälligen Index j zwischen Anfang der Liste und deiner aktuellen Position i
                int j = random.Next(i + 1);

                // 3. Vertausche Eintrag i mit j
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
