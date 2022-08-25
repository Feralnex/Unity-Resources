# Resources
Resources asset introduces an easy way to load data from the Resources folder to the device memory which can be accessed through predefined enumeration types during runtime.

## About
Asset is based on the use of the generic Resources<T, Y> class, in which the first parameter must be of type enum. The constructor of the Resources<T, Y> class takes the path to the resources.

The Resources<T, Y>.Load() method is used to load resources from disk into memory. It is recommended to use this method when loading a scene or asynchronously in the background when access to these resources is not required.

The Resources<T, Y>.StatusChanged event provides an argument of the ResourcesStatus enumeration type that includes:
- Loading - informs that resources are currently being loaded from disk into memory,
- Empty (default) - informs that given resources are empty (not loaded or not found on disk),
- Filled - informs that the given resources have been loaded from disk into memory.

By default loading resources from many files from disk is done by automatically searching for files with names such as value names of a given enumeration type. So it is important that the values in the given enumeration type match the names of the files in the given folder (this behaviour can be changed by overriding protected Resources<T, Y>.LoadResources() method).

The Resources<T, Y> class returns a reference to an object or a copy of it if it implements the ICloneable interface.

The implmenetations of the Prefabs<T> and JsonObject<T, Y> classes are provided in the basic asset configuration:
- The Prefabs<T> class allows you to load prefabs from the disk into the device memory.
- The JsonObject<T, Y> class allows you to load serialized objects into the device memory using the Newtonsoft.Json library from disk. When creating the JsonObjects<T, Y> object, it is possible to decide whether the serialized objects are in a single file or in many separate files (boolean singleFile, false by default). Reading data from a single file is based on Dictionary<T, Y> deserialization.

Unity version:
- 2021.2.7f1.

Packages used:
- Newtonsoft.Json.

## Usage
For easy access to resources kept in memory from the level of the entire project it is recommended to put all instances of classes that inherit from Resources<T, Y> in a static class. So if we want to "hide" the default access to UnityEngine.Resources, it is recommended to create your own static Resources class in the project.

Example from Example/Scripts/Resources.cs:

```cs
static class Resources
{
    public static Prefabs<ObjectId> Prefabs { get; private set; }
    public static JsonObjects<QuestId, Quest> Quests { get; private set; }

    static Resources()
    {
        Prefabs = new Prefabs<ObjectId>(Consts.PREFABS_PATH);
        Quests = new JsonObjects<QuestId, Quest>(Consts.QUESTS_PATH);
    }

    public static void Load()
    {
        Prefabs.Load();
        Quests.Load();
    }
}
```

Loading resources into the device memory will take place after calling the Resources.Load() method.

An exmaple of using the asset based on the code above:

```cs
class Example : MonoBehaviour
{
    protected void Awake()
    {
        Resources.Prefabs.StatusChanged += OnPrefabsStatusChanged; // Subscribe to Resources.Prefabs.StatusChanged event

        Resources.Load(); // Load resources into memory

        if (!Resources.Quests.IsEmpty && // Check if Resources.Quests are not empty
            Resources.Quests.Loaded) // Check if Resources.Quests are loaded
        {
            if (Resources.Quests.TryGet(QuestId.KillHounds, out Quest quest)) // Try to get Quest with id "KillHounds"
            {
                Debug.Log("Name: " + quest.Name);
            }
        }
    }

    private void OnPrefabsStatusChanged(ResourcesStatus status)
    {
        switch (status)
        {
            case ResourcesStatus.Loading:
                Debug.Log("Loading prefabs...");

                break;
            case ResourcesStatus.Empty:
                Debug.Log("Prefabs are empty.");

                break;
            case ResourcesStatus.Filled: // If Resources.Prefabs are loaded into memory
                Debug.Log("Prefabs loaded.");

                GameObject cube = Resources.Prefabs[ObjectId.Cube]; // Get prefab with id "Cube"

                Instantiate(cube, Vector3.zero, Quaternion.identity); // Instantiate Cube

                break;
        }
    }
}
```