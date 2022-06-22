using System;
using UnityEngine;

class Prefabs<T> : Resources<T, GameObject> where T: Enum
{
    public Prefabs(string path) : base(path) { }

    protected override bool TryLoad(T itemId, out GameObject item)
    {
        string path = Path + itemId;

        item = UnityEngine.Resources.Load<GameObject>(path);

        if (item != null) return true;
        else return false;
    }
}