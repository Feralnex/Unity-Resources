using System;
using System.Collections.Generic;

abstract class Resources<T, Y> where T : Enum
{
    public event Action<ResourcesStatus> StatusChanged;

    private readonly Dictionary<T, Y> _items;

    protected string Path { get; set; }

    public IReadOnlyDictionary<T, Y> Items => _items;
    public IReadOnlyCollection<T> Keys => _items.Keys;
    public IReadOnlyCollection<Y> Values => _items.Values;
    public bool Loaded { get; protected set; }
    public bool IsEmpty => _items.Count == 0;

    public Y this[T itemId]
    {
        get
        {
            Y item = _items[itemId];

            if (item is ICloneable cloneable) return (Y)cloneable.Clone();

            return item;
        }
    }

    public Resources(string path)
    {
        _items = new Dictionary<T, Y>();
        Path = path;
        Loaded = false;
    }

    public virtual void Load()
    {
        OnStatusChanged(ResourcesStatus.Loading);

        _items.Clear();

        if (typeof(T).BaseType != typeof(Enum)) return;

        LoadResources();

        if (IsEmpty) OnStatusChanged(ResourcesStatus.Empty);
        else OnStatusChanged(ResourcesStatus.Filled);

        Loaded = true;
    }

    public void Add(T itemId, Y item)
    {
        _items.Add(itemId, item);
    }

    public virtual bool TryGet(T itemId, out Y item)
    {
        if (_items.TryGetValue(itemId, out item))
        {
            if (item is ICloneable cloneable) item = (Y)cloneable.Clone();

            return true;
        }

        return false;
    }

    protected virtual void LoadResources()
    {
        foreach (T itemId in Enum.GetValues(typeof(T)) as T[])
        {
            if (TryLoad(itemId, out Y item)) Add(itemId, item);
        }
    }

    protected abstract bool TryLoad(T itemId, out Y item);

    protected void OnStatusChanged(ResourcesStatus status)
    {
        StatusChanged?.Invoke(status);
    }
}