using System.Collections;
using System.Collections.Generic;
using Case1;
using UnityEngine;

public interface MatchingNeighboursStrategy 
{
    bool Search(MatchingChecker data,int minRequiredNeighbours,int x,int y,out HashSet<Vector2Int> result);
    
}
