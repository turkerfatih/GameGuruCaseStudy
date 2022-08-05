using System.Collections;
using System.Collections.Generic;
using Case1;
using UnityEngine;

public class DFSMatchingNeighboursStrategy: MatchingNeighboursStrategy
{
    public DFSMatchingNeighboursStrategy()
    {
        visited = new HashSet<Vector2Int>();
        stack = new Stack<Vector2Int>();
    }
    
    private readonly HashSet<Vector2Int> visited;
    private readonly Stack<Vector2Int> stack;

    private readonly Vector2Int[] indexes =
    {
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
    };
    
    public bool Search(MatchingChecker data,int minRequiredNeighbours,int x,int y,out HashSet<Vector2Int> result)
    {
        DepthFirstSearch(data,x,y);
        if (visited.Count < minRequiredNeighbours)
        {
            result = null;
            return false;
        }
        result = visited;
        return true;
    }
    
    

    private void DepthFirstSearch(MatchingChecker data, int x, int y)
    {
        visited.Clear();
        stack.Clear();
        stack.Push(new Vector2Int(x,y));
        while (stack.Count>0)
        {
            
            var current = stack.Pop();
            if (!IsValid(current, data))
            {
                continue;
            }

            visited.Add(current);
            foreach (var index in indexes)
            {
                var neighbor = new Vector2Int(current.x + index.x,  current.y + index.y);
                stack.Push(neighbor);
            }
        }
    }

    private bool IsValid(Vector2Int current, MatchingChecker data)
    {
        if (current.x < 0 || current.y < 0 ||
            current.x >= data.Width || current.y >= data.Height)
        {
            return false;
        }

        if (visited.Contains(current))
        {
            return false;
        }
        
        if (data.IsFilled(current.x,current.y))
        {
            return true;
        }
        return false;
    }





}
