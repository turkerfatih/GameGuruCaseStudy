using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell:MatchingFillChecker
{
    private int value;
    public GameObject Item;
    public bool IsFilled
    {
        get { return value == 1; }
    }

    public void Empty()
    {
        Item = null;
        value = 0;
    }

    public void Fill(GameObject item)
    {
        Item = item;
        value = 1;
    }

    public void TemporaryFill()
    {
        value = 1;
    }
}
