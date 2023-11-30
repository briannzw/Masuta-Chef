using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

namespace Crate.Combine
{
    using Cooking;
    using Loot.Object;
    using Player.CompanionSlot;
    using NPC.Companion;
    using Spawner;

    public class CombineManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject ingredientPrefab;
        [SerializeField] private GameObject medkitPrefab;

        [Header("UI References")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private Image itemIcon;

        [Header("UI Icon Sprites")]
        [SerializeField] private Sprite medkitIcon;
        [SerializeField] private Sprite weaponRandomIcon;
        [SerializeField] private Sprite companionRandomIcon;
        [SerializeField] private SerializedDictionary<CrateColor, Sprite> WeaponIcons;
        [SerializeField] private SerializedDictionary<CrateColor, Sprite> CompanionsIcons;

        [Header("Position")]
        [SerializeField] private Vector3 offset;

        [Header("Dictionaries")]
        [SerializeField] private SerializedDictionary<CrateColor, Ingredient> Ingredients;
        [SerializeField] private SerializedDictionary<CrateColor, GameObject> Weapons;
        [SerializeField] private SerializedDictionary<CrateColor, NavMeshSpawner> CompanionsSpawner;

        public bool Combine(List<CrateColor> colors)
        {
            NavMesh.SamplePosition(transform.position, out var hit, 10f, 1 << LayerMask.GetMask("Walkable"));

            hit.position += offset;

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
                    companion = CompanionsSpawner[colors[0]].Spawn()[0];
                }
                else
                {
                    companion = CompanionsSpawner[(CrateColor)Random.Range(0, System.Enum.GetNames(typeof(CrateColor)).Length)].Spawn()[0];
                }
                companion.GetComponent<Character.Character>().Reset();
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

        public void UpdateCombine(List<CrateColor> colors)
        {
            if(colors == null || colors.Count == 0)
            {
                canvas.SetActive(false);
                return;
            }

            canvas.SetActive(true);

            if (colors.Count == 1)
            {
                itemIcon.sprite = Ingredients[colors[0]].Icon;
            }
            else if (colors.Count == 2)
            {
                if (IsSameColor(colors)) itemIcon.sprite = medkitIcon;
                else
                    // CHANGE SPRITE TO X (Uncraftable)
                    itemIcon.sprite = null;
            }
            else if (colors.Count == 3)
            {
                if (IsSameColor(colors)) itemIcon.sprite = WeaponIcons[colors[0]];
                else
                    // CHANGE SPRITE TO ? (Random)
                    itemIcon.sprite = weaponRandomIcon;
            }
            else if (colors.Count == 4)
            {
                if (IsSameColor(colors)) itemIcon.sprite = CompanionsIcons[colors[0]];
                else
                    // CHANGE SPRITE TO ? (Random)
                    itemIcon.sprite = companionRandomIcon;
            }
        }
    }
}