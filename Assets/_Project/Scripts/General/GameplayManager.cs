using System;
using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class GameplayManager : LocalSingleton<GameplayManager>
    {
        private void Awake()
        {
            // SceneHandler.Instance.RegisterSceneChanged();
        }

        private void OnDisable()
        {
            // SceneHandler.Instance.RemoveSceneChanged();
        }
    }
}