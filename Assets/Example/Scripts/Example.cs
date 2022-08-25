using UnityEngine;

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