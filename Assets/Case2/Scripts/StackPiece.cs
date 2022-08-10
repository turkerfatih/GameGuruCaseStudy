using System;
using DG.Tweening;
using UnityEngine;

namespace Case2
{
    public class StackPiece : MonoBehaviour
    {
        [HideInInspector]
        public Material Material;
        private void Awake()
        {
            Material = GetComponent<MeshRenderer>().material;
        }
    }
}