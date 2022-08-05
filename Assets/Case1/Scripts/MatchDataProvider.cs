using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MatchDataProvider<T>
{
    public T GetValue(int x, int y);
    public void SetValue(int x, int y, T value);
    public void Resize(int width, int height);

    public int Height { get; }
    public int Width { get; }
}

