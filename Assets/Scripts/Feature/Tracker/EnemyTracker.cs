using HUD;
using UnityEngine;

namespace Tracker
{
    public class EnemyTracker : MonoBehaviour
    {
        private Renderer m_renderer;
        private Camera m_camera;

        private Vector3 screenPos;
        private bool onScreen;

        public IndicatorHUD indicator;

        void Start()
        {
            m_renderer = GetComponentInChildren<Renderer>();
            m_camera = Camera.main;

            InvokeRepeating(nameof(CheckVisibility), 0f, 1f);
        }
        public void CheckVisibility()
        {
            //Check Visibility

            screenPos = m_camera.WorldToScreenPoint(transform.position);
            onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

            if (onScreen && m_renderer.isVisible)
            {
                //Visible
                indicator.ResetTimer();
            }
            else
            {
                //NotVisible
            }
        }
    }
}