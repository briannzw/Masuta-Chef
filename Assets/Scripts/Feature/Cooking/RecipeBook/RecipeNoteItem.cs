using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking.RecipeBook
{
    using Cooking.Recipe;

    public class RecipeNoteItem : MonoBehaviour
    {
        public RecipeBookManager manager;
        private Recipe recipe;

        [Header("Background")]
        public Button NoteButton;

        [Header("Recipe")]
        public Image RecipeIcon;
        public Image RecipeLockedIcon;
        public TMP_Text RecipeName;

        private void Reset()
        {
            NoteButton.interactable = true;

            RecipeLockedIcon.gameObject.SetActive(false);
            RecipeIcon.color = Color.white;
        }

        public void Set(Recipe recipe)
        {
            Reset();

            this.recipe = recipe;
            RecipeIcon.sprite = recipe.Icon;
            RecipeName.text = recipe.name;
        }

        public void Lock(RecipeData data = null)
        {
            this.recipe = null;

            NoteButton.interactable = false;
            RecipeIcon.color = Color.black;
            RecipeLockedIcon.gameObject.SetActive(true);
            RecipeName.text = "???";

            if (data != null) RecipeName.text += $"\n({data.CurrentBlueprint} / {data.NeededBlueprint})";
        }

        public void OnClick()
        {
            manager.SetUI(recipe);
        }
    }
}