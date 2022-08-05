using System.Collections.Generic;
using UnityEngine;

namespace Case1
{
    public class MatchingNeighboursFinder
    {
        private MatchingNeighboursStrategy currentStrategy;
        private int minimumRequiredNeighbours;

        public MatchingNeighboursFinder(int minRequiredNeighbours)
        {
            minimumRequiredNeighbours = minRequiredNeighbours;
            currentStrategy = new DFSMatchingNeighboursStrategy();
        }

        public void SetStrategy(MatchingNeighboursStrategy strategy)
        {
            currentStrategy = strategy;
        }

        public void SetMinimumRequiredNeighbours(int val) => minimumRequiredNeighbours = val;

        public bool Search(MatchingChecker data,int x,int y,out  HashSet<Vector2Int> result)
        {
            return currentStrategy.Search(data,minimumRequiredNeighbours,x,y, out result);
        }

    }    
}


