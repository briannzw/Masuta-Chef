using System;
using Unity.AI.Navigation;
using UnityEngine;

namespace Level
{
    using AYellowpaper.SerializedCollections;
    using Cooking;
    using Cooking.Recipe;
    using Level.Area;
    using Result;
    using System.Linq;
    using Wave;

    public class LevelManager : MonoBehaviour
    {
        public LevelData CurrentLevel;

        [Header("References")]
        public WaveManager waveManager;
        public Transform PlayerTransform;
        public NavMeshSurface[] surfaces;
        public BattleEndResult resultUI;
        public Transform lightingTransform;

        [Header("Data")]
        [SerializeField] protected SerializedDictionary<string, int> characterDied = new();
        protected BattleResult result = new();

        private LevelAreaInfo levelAreaInfo;

        #region C# Events
        public Action OnLevelWin;
        public Action OnLevelLose;
        #endregion

        private void Awake()
        {
            SpawnLevelArea();
            SpawnPlayer();
            GenerateNavMesh();
        }

        private void GenerateNavMesh()
        {
            foreach (var surface in surfaces)
            {
                surface.BuildNavMesh();
            }
            Debug.Log("Navmesh Built");
        }

        private void SpawnLevelArea()
        {
            GameObject levelArea = Instantiate(CurrentLevel.LevelAreaPrefab, transform.position, Quaternion.identity);
            levelAreaInfo = levelArea.GetComponent<LevelAreaInfo>();
            waveManager.Spawners = levelAreaInfo.EnemySpawners;
            waveManager.disasterSpawner = levelAreaInfo.DisasterSpawner;
            lightingTransform.eulerAngles = levelAreaInfo.sunRotation;
        }

        private void SpawnPlayer()
        {
            // Spawn player at the specified position
            PlayerTransform.position = CurrentLevel.PlayerSpawnPoint;
        }

        public void GameStarted() => result.startTime = DateTime.Now;

        public void CharacterDied(Character.Character chara)
        {
            if(!characterDied.ContainsKey(chara.tag)) characterDied.Add(chara.tag, 0);
            characterDied[chara.tag]++;

            // Action
            if (chara.CompareTag("Enemy"))
            {
                GameManager.Instance.OnEnemiesKilled?.Invoke();
                waveManager.EnemyDied();
            }

            // If All enemy in all Waves killed
            if (chara.CompareTag("Enemy") && characterDied["Enemy"] >= waveManager.TotalEnemies)
            {
                Debug.Log("[Game Over] Player Win (All Waves Cleared)");

                CalculateReward(true);
                result.clearTime = DateTime.Now;
                result.clearDuration = result.clearTime - result.startTime;
                result.isBestTime = GameManager.Instance.SaveManager.SaveData.Add(CurrentLevel.name, result.clearDuration);
                ShowResultUI(true);
                // SAVE
                OnLevelWin?.Invoke();
            }

            if (chara.CompareTag("Player") && characterDied["Player"] >= 1)
            {
                Debug.Log("[Game Over] Player Lose (Health < 0)");

                CalculateReward(false);
                ShowResultUI(false);
                // SAVE
                OnLevelLose?.Invoke();
            }
        }

        private void CalculateReward(bool victory)
        {
            foreach(var ingredient in result.ingredientsCollected.ToArray())
            {
                result.ingredientsCollected[ingredient.Key] = Mathf.CeilToInt(ingredient.Value * (victory ? 1 : CurrentLevel.LoseDropMultiplier));

                int remainder = GameManager.Instance.SaveManager.SaveData.Add(ingredient.Key, result.ingredientsCollected[ingredient.Key]);
                result.ingredientsCollected[ingredient.Key] -= remainder;
                if (result.ingredientsCollected[ingredient.Key] < 0) result.ingredientsCollected[ingredient.Key] = 0;
            }

            foreach(var blueprint in result.blueprintsCollected.ToArray())
            {
                result.blueprintsCollected[blueprint.Key] = Mathf.CeilToInt(blueprint.Value * (victory ? 1 : CurrentLevel.LoseDropMultiplier));

                int remainder = GameManager.Instance.SaveManager.SaveData.Add(blueprint.Key, result.blueprintsCollected[blueprint.Key]);
                result.blueprintsCollected[blueprint.Key] -= remainder;
                if (result.blueprintsCollected[blueprint.Key] < 0) result.blueprintsCollected[blueprint.Key] = 0;
            }
        }

        private void ShowResultUI(bool victory)
        {
            Time.timeScale = 0f;
            // Canvas
            resultUI.transform.parent.gameObject.SetActive(true);
            resultUI.Set(victory, result);
        }

        public void IngredientCollected(Ingredient ingredient, int count)
        {
            if (!result.ingredientsCollected.ContainsKey(ingredient))
                result.ingredientsCollected.Add(ingredient, 0);

            result.ingredientsCollected[ingredient] += count;
        }

        public void BlueprintCollected(Recipe recipe, int count)
        {
            if (!result.blueprintsCollected.ContainsKey(recipe))
                result.blueprintsCollected.Add(recipe, 0);

            result.blueprintsCollected[recipe] += count;
        }

        public void DisableCrateSpawn()
        {
           if(levelAreaInfo != null) levelAreaInfo.CrateSpawner.gameObject.SetActive(false);
        }

        public void EnableCrateSpawn()
        {
            levelAreaInfo.CrateSpawner.gameObject.SetActive(true);
        }

        public int GetCurrentEnemyCount(string characterTag)
        {
            if (characterDied.ContainsKey(characterTag))
            {
                return characterDied[characterTag];
            }
            else
            {
                return 0;
            }
        }
    }
}
