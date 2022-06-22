using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

class JsonObjects<T, Y> : Resources<T, Y> where T: Enum
{
    protected bool SingleFile { get; set; }

    public JsonObjects(string path, bool singleFile = false) : base(path)
    {
        SingleFile = singleFile;
    }

    protected override void LoadResources()
    {
        if (!SingleFile) base.LoadResources();
        else
        {
            if (TryLoad(out Dictionary<T, Y> items))
            {
                foreach (KeyValuePair<T, Y> item in items)
                {
                    Add(item.Key, item.Value);
                }
            }
        }
    }

    protected override bool TryLoad(T itemId, out Y item)
    {
        string path = Path + itemId;
        TextAsset json = UnityEngine.Resources.Load<TextAsset>(path);

        item = default;

        if (json == null) return false;

        item = JsonConvert.DeserializeObject<Y>(json.text);

        if (item != null) return true;
        else return false;
    }

    protected virtual bool TryLoad(out Dictionary<T, Y> items)
    {
        TextAsset json = UnityEngine.Resources.Load<TextAsset>(Path);

        items = default;

        if (json == null) return false;

        items = JsonConvert.DeserializeObject<Dictionary<T, Y>>(json.text);

        if (items != null) return true;
        else return false;
    }
}