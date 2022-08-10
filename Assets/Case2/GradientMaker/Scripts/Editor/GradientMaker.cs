using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GradientMaker : MonoBehaviour
{
    public CustomGradient Palette;
    public Material Material;
    public CinemachineTargetGroup TargetGroup;
    private MeshRenderer[] cubes;
    private int length = 7;
    private Color sourceColor=Color.white;
    private Color targetColor=Color.white;
    private IEnumerator iterator;
    private List<PairGradient> gradients;

    private void Start()
    {
        iterator = Iterate();
        iterator.MoveNext();
        ReCreateCubes();
        Colorize();
    }

    public void Discard()
    {
        Next();
    }
    private void Next()
    {
        if (!iterator.MoveNext())
        {
            Debug.LogWarning("Finished");
            return;
        }
        ReCreateCubes();
        Colorize();
    }

    public void Save()
    {
        if (gradients == null)
            gradients = new List<PairGradient>();
        PairGradient source = GetOrCreatePairGradient(sourceColor);
        PairGradient target = GetOrCreatePairGradient(targetColor);
        source.Add(target,length);
        foreach (var gradient in gradients)
        {
            EditorUtility.SetDirty(gradient);
        }
        AssetDatabase.SaveAssets();
        
        Next();
    }

    private PairGradient GetOrCreatePairGradient(Color color)
    {
        PairGradient result = null;
        foreach (var gradient in gradients)
        {
            if (gradient.Color.Equals(color))
            {
                result = gradient;
                break;
            }
        }

        if (result == null)
        {
            result = ScriptableObject.CreateInstance<PairGradient>();
            result.Color = color;
            gradients.Add(result);
            var path = AssetDatabase.GetAssetPath(Palette);
            path = path.Substring(0, path.LastIndexOf("/"));
            AssetDatabase.CreateAsset(result,path+"Gradient"+result.GetInstanceID()+".asset");
        }
        return result;
    }

    private IEnumerator Iterate()
    {
        for (int i = 0; i < Palette.Colors.Length-1; i++)
        {
            for (int j = i+1; j < Palette.Colors.Length; j++)
            {
                sourceColor = Palette.Colors[i];
                targetColor = Palette.Colors[j];
                yield return true;
            }
        }
    }

    private MeshRenderer CreateACube(float z)
    {
       var mesh= GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshRenderer>();
       mesh.transform.SetParent(transform);
       mesh.transform.localPosition = new Vector3(0, 0, z);
       mesh.material = Instantiate(Material);
       return mesh;
    }
    
    private void ReCreateCubes() {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        cubes = new MeshRenderer[length];
        for (int i = 0; i < length; i++)
        {
           cubes[i]= CreateACube(i);
        }

        TargetGroup.m_Targets[0].target = cubes[0].transform;
        TargetGroup.m_Targets[1].target = cubes[cubes.Length-1].transform;
    }

    private void Colorize()
    {
        float len = length;
        for (int i = 0; i < length; i++)
        {
            cubes[i].material.color = Color.Lerp(sourceColor,targetColor,i/len);
        }
    }


    public void LengthChanged(string input)
    {
        if (int.TryParse(input, out var len))
        {
            if (length != len)
            {
                length = len;
                ReCreateCubes();
                Colorize();
            }
        }
    }

}
