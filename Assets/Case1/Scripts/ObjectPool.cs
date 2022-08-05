using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private readonly Dictionary<int, GameObject> activeItems;
    private readonly Stack<GameObject> passiveItems;
    private readonly GameObject prefab;
    public ObjectPool(GameObject prefab,int preloadAmount)
    {
        this.prefab = prefab;
        passiveItems = new Stack<GameObject>(preloadAmount);
        activeItems = new Dictionary<int, GameObject>(preloadAmount);
        Warm(preloadAmount);
    }
    private void Warm(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            CreatePassiveItem();
        }
    }
    private void CreatePassiveItem()
    {
        var go = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        go.SetActive(false);
        passiveItems.Push(go);
    }
    public GameObject GetItem()
    {
        if (passiveItems.Count == 0)
        {
            CreatePassiveItem();
        }
        GameObject go = passiveItems.Pop();
        activeItems.Add(go.GetInstanceID(), go);
        return go;
    }
    
    public void PutItem(GameObject go)
    {
        int id = go.GetInstanceID();
        if (!activeItems.ContainsKey(id))
        {
            throw new System.Exception("object pool does not contain" + go.name);
        }

        go.SetActive(false);
        activeItems.Remove(id);
        passiveItems.Push(go);
    }

    public void PutBackAll()
    {
        foreach (var activeItem in activeItems)
        {
            passiveItems.Push(activeItem.Value);
            activeItem.Value.SetActive(false);
        }
        activeItems.Clear();
    }
}
