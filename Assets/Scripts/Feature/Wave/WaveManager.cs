using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace Wave
{
    using AYellowpaper.SerializedCollections;
    using Level;
    using Loot;
    using Spawner;
    using Character;
    using Character.Stat;
    using Weapon;
    using Character.Hit;
    using Player.Controller;
    using System.Linq;
    using HUD;
    using Tracker;

    public class WaveManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelManager levelManager;
        protected LevelData LevelData;
        public SerializedDictionary<GameObject, List<NavMeshSpawner>> Spawners = new();

        [Header("UI References")]
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text waveEnemiesText;
        [SerializeField] private TMP_Text nextWaveText;
        [SerializeField] private float incomingWaveDuration = 5f;
        [SerializeField] private IndicatorHUD indicatorHUD;

        [Header("Weapon Selection")]
        [SerializeField] private bool startWaveAfterSelect = false;
        [SerializeField] private PlayerWeaponController weaponController;
        [SerializeField] private GameObject weaponSelectionParent;

        [Header("Disaster Mode")]
        [SerializeField] private Character disasterChara;
        [SerializeField] private Weapon disasterWeapon;
        [Space]
        [SerializeField] private GameObject disasterPrefab;
        [SerializeField] private GameObject disasterIcon;
        [SerializeField] private GameObject disasterText;
        [SerializeField] private TMP_Text disasterWarningText;

        private int currentWaveIndex = 0;
        private int currentEnemyCount = 0;

        private HashSet<GameObject> spawnedEnemies = new();

        private bool disasterStopped;

        public int TotalEnemies
        {
            get
            {
                int total = 0;
                foreach (var wave in LevelData.Waves)
                {
                    total += wave.TotalEnemies;
                }
                return total;
            }
        }

        private void Awake()
        {   
            LevelData = levelManager.CurrentLevel;
            levelManager.DisableCrateSpawn();
        }

        private void Start()
        {
            if (startWaveAfterSelect)
            {
                weaponController.OnWeaponChanged += FirstWave;
                return;
            }

            FirstWave();
        }

        private void FirstWave()
        {
            levelManager.GameStarted();

            StartWave(LevelData.Waves[0]);
            StartCoroutine(DoDisaster());
            levelManager.EnableCrateSpawn();

            if (startWaveAfterSelect)
            {
                weaponController.OnWeaponChanged -= FirstWave;
                weaponSelectionParent.SetActive(false);
            }

            indicatorHUD.IsGameStarted = true;
        }

        public void EnemyDied()
        {
            currentEnemyCount++;

            waveEnemiesText.text = $"Enemies Left: {LevelData.Waves[currentWaveIndex].TotalEnemies - currentEnemyCount}";

            if (currentEnemyCount == LevelData.Waves[currentWaveIndex].TotalEnemies)
            {
                currentEnemyCount = 0;
                if (currentWaveIndex >= LevelData.Waves.Count - 1) return;
                StartCoroutine(CountdownWave());
            }
        }

        private void StartWave(Wave wave)
        {
            // UI
            waveText.text = $"Wave {currentWaveIndex + 1}";
            waveEnemiesText.text = $"Enemies Left: {wave.TotalEnemies}";

            foreach (var enemy in wave.EnemyList)
            {
                int spawnMod = enemy.Value % Spawners[enemy.Key.Prefab].Count;
                int spawnCount = enemy.Value / Spawners[enemy.Key.Prefab].Count;
                for (int i = 0; i < Spawners[enemy.Key.Prefab].Count; i++)
                {
                    if (i < spawnMod) StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount + 1, wave.SpawnPerInterval, wave.SpawnInterval, enemy.Key.StatsPreset));
                    else StartCoroutine(SpawnEnemies(Spawners[enemy.Key.Prefab][i], spawnCount, wave.SpawnPerInterval, wave.SpawnInterval, enemy.Key.StatsPreset));
                }
            }
        }

        private IEnumerator SpawnEnemies(NavMeshSpawner spawner, int total, int spawnCount, float interval, StatsPreset enemyPreset)
        {
            float timer = 0f;
            while (total > 0)
            {
                timer += Time.deltaTime;
                if(timer >= interval)
                {
                    List<GameObject> enemies = spawner.Spawn(total < spawnCount ? total : spawnCount);

                    foreach (var enemy in enemies)
                    {
                        // Assign Preset
                        if(enemy.GetComponent<Character>() == null) enemy.AddComponent<Character>();
                        enemy.GetComponent<Character>().StatsPreset = enemyPreset;
                        enemy.GetComponent<Character>().Reset();

                        // Assign Loot Drop
                        if(enemy.GetComponent<LootDropController>() == null) enemy.AddComponent<LootDropController>();
                        enemy.GetComponent<LootDropController>().lootChance = LevelData.EnemyLootDrop;

                        // Assign Screen Tracker
                        if (enemy.GetComponent<EnemyTracker>() == null) enemy.AddComponent<EnemyTracker>();
                        enemy.GetComponent<EnemyTracker>().indicator = indicatorHUD;

                        if(!spawnedEnemies.Contains(enemy)) spawnedEnemies.Add(enemy);
                    }
                    total -= spawnCount;
                    timer = 0f;
                }

                yield return null;
            }
        }

        private IEnumerator CountdownWave()
        {
            nextWaveText.gameObject.SetActive(true);

            int time = LevelData.WaveWaitTime;
            disasterStopped = true;
            while(time > 0)
            {
                nextWaveText.text = $"Next wave in\n{time}";
                yield return new WaitForSeconds(1f);
                time--;
            }
            disasterStopped = false;

            currentWaveIndex++;
            StartWave(LevelData.Waves[currentWaveIndex]);

            nextWaveText.text = "INCOMING WAVE";

            yield return new WaitForSeconds(incomingWaveDuration);

            nextWaveText.gameObject.SetActive(false);
        }

        private IEnumerator DoDisaster()
        {
            disasterWeapon.OnEquip(disasterChara);
            disasterPrefab.GetComponent<HitController>().Initialize(disasterWeapon, LevelData.DisasterDamageScaling);
            yield return new WaitForSeconds(LevelData.DisasterStartTime);

            float time = Random.Range(LevelData.DisasterMinInterval, LevelData.DisasterMaxInterval);

            while (true)
            {
                while (time > 0f)
                {
                    time -= (!disasterStopped ? Time.deltaTime : 0f);

                    if (time <= 10f)
                    {
                        disasterText.SetActive(true);

                        disasterWarningText.gameObject.SetActive(!disasterStopped);
                        disasterWarningText.text = $"Disaster in\n{Mathf.Round(time)}";
                    }

                    yield return null;
                }

                disasterIcon.SetActive(true);

                // Spawn Disaster
                GameObject disasterGO = Instantiate(disasterPrefab);

                disasterWarningText.gameObject.SetActive(false);

                yield return new WaitForSeconds(LevelData.DisasterDuration);

                Destroy(disasterGO);
                disasterIcon.SetActive(false);
                disasterText.SetActive(false);

                time = Random.Range(LevelData.DisasterMinInterval, LevelData.DisasterMaxInterval);
            }
        }

        public Transform NearestEnemy()
        {
            if (spawnedEnemies.Count == 0) return null;

            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = GameManager.Instance.PlayerTransform.position;
            foreach(var enemy in spawnedEnemies)
            {
                if (enemy == null || !enemy.activeSelf) continue;

                Vector3 directionToTarget = enemy.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = enemy.transform;
                }
            }

            return bestTarget;
        }

        #region NavMeshAgent
        public void DisableWave()
        {
            disasterStopped = true;
            levelManager.DisableCrateSpawn();
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.activeSelf)
                {
                    if (enemy.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    {
                        navMeshAgent.isStopped = true;
                    }
                }
            }
        }
        public void EnableWave()
        {
            disasterStopped = false;
            levelManager.EnableCrateSpawn();
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null && enemy.activeSelf)
                {
                    if (enemy.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    {
                        navMeshAgent.isStopped = false;
                    }
                }
            }
        }
        #endregion        
    }
}
