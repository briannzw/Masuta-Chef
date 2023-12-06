using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class IndicatorHUD : MonoBehaviour
    {
        public bool IsGameStarted = false;
        public float showWhen = 5f;
        private float showTimer = 0f;

        [Header("References")]
        [SerializeField] private RectTransform arrowUIObject;
        private Image arrowImage;

        private Transform target;
        private bool fetchTarget = true;

        private void Awake()
        {
            arrowImage = arrowUIObject.GetComponent<Image>();
            arrowImage.enabled = false;
        }

        public void ResetTimer()
        {
            showTimer = 0f;
            fetchTarget = true;
            arrowImage.enabled = false;
        }

        private void Update()
        {
            if (!IsGameStarted) return;

            showTimer += Time.deltaTime;

            if(showTimer > showWhen)
            {
                if (fetchTarget)
                {
                    target = GameManager.Instance.LevelManager.waveManager.NearestEnemy();
                    fetchTarget = false;
                    if(target != null) arrowImage.enabled = true;
                }

                if (target == null) return;

                var targetPosLocal = Camera.main.transform.InverseTransformPoint(target.position);
                var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg;
                arrowUIObject.eulerAngles = new Vector3(0, 0, targetAngle);
            }
        }
    }
}