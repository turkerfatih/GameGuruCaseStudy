using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Case1/Create Case1 Config", fileName = "Case1Config", order = 0)]
public class Case1Config : ScriptableObject
{
    public int MinimumMatchCount;
    public int MaximumMatchCount;
    public int MinimumGridSize;
    public int MaximumGridSize;
    public float TileSize;
    public GameObject TilePrefab;
    public int TilePrefabPoolSize;
    public GameObject ItemPrefab;
    public int ItemPrefabPoolSize;
    
}
