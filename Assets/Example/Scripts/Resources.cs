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