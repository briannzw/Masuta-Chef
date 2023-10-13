namespace Wave
{
    using AYellowpaper.SerializedCollections;

    [System.Serializable]
    public class Wave
    {
        public SerializedDictionary<EnemySettings, int> EnemyList;
        public float Time;
        public float SpawnInterval = .5f;
        public int SpawnPerInterval = 3;
    }
}