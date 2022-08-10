using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Pair Gradient", fileName = "PairGradient", order = 0)]
public class PairGradient : ScriptableObject
{
    public Color Color;
    public List<PairedGradient> PairGradients;

    public void Add(PairGradient pairGradient,int length)
    {
        if (PairGradients == null)
            PairGradients = new List<PairedGradient>();
        foreach (var pairedGradient in PairGradients)
        {
            if (pairedGradient.Pair.Color.Equals(pairGradient.Color))
            {
                return;
            }
        }
        PairGradients.Add(new PairedGradient()
        {
            Pair = pairGradient,
            Length = length,
        });
    }
}

[Serializable]
public class PairedGradient
{
    public PairGradient Pair;
    public int Length;
}
