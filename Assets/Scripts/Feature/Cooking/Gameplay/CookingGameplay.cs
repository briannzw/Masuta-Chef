using System;
using UnityEngine;

namespace Cooking.Gameplay
{
    public abstract class CookingGameplay : MonoBehaviour
    {
        #region Actions
        public Action OnCookingSuccess;
        public Action OnCookingFailed;
        public Action OnCookingHit;
        public Action OnCookingMissed;
        #endregion
    }
}