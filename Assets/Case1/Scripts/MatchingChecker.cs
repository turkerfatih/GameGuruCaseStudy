using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MatchingChecker 
{
    public bool IsFilled(int x, int y);
    public int Height { get; }
    public int Width { get; }
}
