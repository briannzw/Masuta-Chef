using UnityEngine;

namespace Save
{
    using Data;
    using MyBox;
    using System;

    public class SaveManager : MonoBehaviour
    {
        public bool IsEncrypted;
        public SaveData SaveData;
        [Header("Statistics")]
        [ReadOnly, SerializeField] private float saveTime;
        [ReadOnly, SerializeField] private float loadTime;
        private IDataPersistence dataPersistence = new DataPersistenceManager();

        public void Save()
        {
            var startTime = DateTime.Now.Ticks;
            if (dataPersistence.WriteData("/data.save", SaveData, IsEncrypted))
            {
                saveTime = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
                Debug.Log("Data successfuly saved!");
            }
        }

        public void Load()
        {
            var startTime = DateTime.Now.Ticks;
            SaveData = dataPersistence.ReadData<SaveData>("/data.save", IsEncrypted);
            loadTime = (DateTime.Now.Ticks - startTime) / TimeSpan.TicksPerMillisecond;
        }
    }
}