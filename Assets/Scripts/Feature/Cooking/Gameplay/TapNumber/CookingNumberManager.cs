using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    using AYellowpaper.SerializedCollections;
    public enum CookingTapNumberDifficulty
    {
        Easy, Medium, Hard
    }
    public class CookingNumberManager : MonoBehaviour
    {
        [Header("Parameters")]
        public float GameTime = 60f;

        [Header("Difficulty")]
        public CookingTapNumberDifficulty CurrentDifficulty;
        public SerializedDictionary<CookingTapNumberDifficulty, int> TargetCount;
        public SerializedDictionary<CookingTapNumberDifficulty, int> TargetMissCount;
        public SerializedDictionary<CookingTapNumberDifficulty, CookingNumberMinMax> NumberDuration;

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
            else if (successTap >= TargetCount[CurrentDifficulty])
            {
                Debug.Log("Result : GOOD"); 
                // TBD
                if(false)
                    Debug.Log("Result : PERFECT");
            }
        }

        public void TapSuccess(int num)
        {
            if (num == lastTapNum + 1)
            {
                successTap++;
                lastTapNum = num;
            }
            else
                TapMissed();
        }

        public void TapMissed()
        {
            missedTap++;
        }
    }
}