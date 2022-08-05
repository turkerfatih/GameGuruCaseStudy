
using System.Collections.Generic;
using UnityEngine;

namespace Case1
{
    public interface MatchingNeighboursStrategy 
    {
        bool Search(MatchingChecker data,int minRequiredNeighbours,int x,int y,out HashSet<Vector2Int> result);
    
    }   
}

