using System.ComponentModel;
using GeneticAlgorithm.Algorithm.Selection;

namespace SmartGen.Types
{
    public enum SelectionType
    {
        [Description("Tournament")] Tournament,
        [Description("Rank")] Rank
    }

    public static class SelectionExtension
    {
        public static Selection GetSelection(SelectionType type, int populationSize, int size)
        {
            switch (type)
            {
                case SelectionType.Rank:
                    return new RankSelection(populationSize);
                case SelectionType.Tournament:
                default:
                    return new TournamentSelection(populationSize, size);
            }
        }
    }
}