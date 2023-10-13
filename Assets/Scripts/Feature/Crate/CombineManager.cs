using System.Collections.Generic;
using UnityEngine;

namespace Crate.Combine
{
    using AYellowpaper.SerializedCollections;
    using Cooking;
    using Loot.Object;
    using Player.CompanionSlot;
    using UnityEngine.AI;

    public class CombineManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject ingredientPrefab;
        [SerializeField] private GameObject medkitPrefab;

        [Header("Dictionaries")]
        [SerializeField] private SerializedDictionary<CrateColor, Ingredient> Ingredients;
        [SerializeField] private SerializedDictionary<CrateColor, GameObject> Weapons;
        [SerializeField] private SerializedDictionary<CrateColor, GameObject> Companions;

        public bool Combine(List<CrateColor> colors)
        {
            NavMesh.SamplePosition(transform.position, out var hit, 10f, 1 << LayerMask.GetMask("Walkable"));

            // Ingredients
            if (colors.Count == 1)
            {
                GameObject lootObj = Instantiate(ingredientPrefab, hit.position, Quaternion.identity);
                lootObj.GetComponent<IngredientLootObject>().Ingredient = Ingredients[colors[0]];
            }
            else if(colors.Count == 2)
            {
                if (IsSameColor(colors))
                    Instantiate(medkitPrefab, hit.position, Quaternion.identity);
                else return false;
            }
            else if(colors.Count == 3)
            {
                if (IsSameColor(colors))
                {
                    Instantiate(Weapons[colors[0]], hit.position, Quaternion.identity);
                }
                else Instantiate(Weapons[(CrateColor)Random.Range(0, System.Enum.GetNames(typeof(CrateColor)).Length)], hit.position, Quaternion.identity);
            }
            else if(colors.Count == 4)
            {
                GameObject companion;
                if (IsSameColor(colors))
                {
                    companion = Instantiate(Companions[colors[0]], hit.position, Quaternion.identity);
                }
                else
                {
                    companion = Instantiate(Companions[(CrateColor)Random.Range(0, System.Enum.GetNames(typeof(CrateColor)).Length)], hit.position, Quaternion.identity);
                }
                GameManager.Instance.PlayerTransform.GetComponent<CompanionSlotManager>().AddCompanion(companion.GetComponent<Companion>());
            }

            return true;
        }

        private bool IsSameColor(List<CrateColor> colors)
        {
            CrateColor singleColor = colors[0];

            for (int i = 1; i < colors.Count; i++)
            {
                if (colors[i] != singleColor) return false;
            }

            return true;
        }
    }
}