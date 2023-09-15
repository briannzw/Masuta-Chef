using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    using AYellowpaper.SerializedCollections;
    public class CookingNumberManager : MonoBehaviour
    {
        [Header("Parameters")]
        public float GameTime = 60f;

        [Header("Difficulty")]
        public CookingDifficulty CurrentDifficulty;
        public SerializedDictionary<CookingDifficulty, int> TargetCount;
        public SerializedDictionary<CookingDifficulty, int> TargetMissCount;
        public SerializedDictionary<CookingDifficulty, CookingDifficultyFrequency> NumberDuration;

        private float timer = 0f;
        private bool gameEnded = false;

        private int successTap = 0;
        private int missedTap = 0;

        private int lastTapNum = 0;

        private void Update()
        {
            if(gameEnded) return;

            timer += Time.deltaTime;

            if (timer > GameTime || missedTap >= TargetMissCount[CurrentDifficulty] || successTap >= TargetCount[CurrentDifficulty]) GameOver();
        }

        private void GameOver()
        {
            gameEnded = true;
            if (missedTap >= TargetMissCount[CurrentDifficulty]) Debug.Log("Result : BAD");
            else if (successTap >= TargetCount[CurrentDifficulty]) Debug.Log("Result : PERFECT"); 
            else if(successTap < TargetCount[CurrentDifficulty]) Debug.Log("Result : GOOD");
        }

        public void TapSuccess(int num)
        {
            if (num == lastTapNum + 1)
                successTap++;
            else
            {
                Debug.Log("Number not ascending : " + lastTapNum);
                missedTap++;
            }
            lastTapNum = num;
        }

        public void TapMissed()
        {
            missedTap++;
        }

        #region GUI
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(10, 10, 500, 200), "Missed Count : " + missedTap);
        }
        #endregion
    }
}