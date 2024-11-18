using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FTKingdom.Utils;

namespace FTKingdom
{
    public class InputManager : LocalSingleton<InputManager>
    {
        [SerializeField] private InputActionAsset gameInput;

        private InputActionMap Gameplay;

        private void Awake()
        {
            Gameplay = gameInput.FindActionMap("Gameplay");
            EnableGameplayInput();
            // SceneHandler.Instance.RegisterSceneChanged(OnChangeChange);
        }

        #region Behavior
        public void EnableGameplayInput()
        {
            Gameplay.Enable();
        }

        public void DisableGameplayInput()
        {
            Gameplay.Disable();
        }
        #endregion Behavior

        #region Register
        public void RegisterPerformed(Command action, Action<InputAction.CallbackContext> callback)
        {
            GetInputActionMapByType(action).performed += callback;
        }

        public void RegisterCanceled(Command action, Action<InputAction.CallbackContext> callback)
        {
            GetInputActionMapByType(action).canceled += callback;
        }

        public void UnregisterPerformed(Command action, Action<InputAction.CallbackContext> callback)
        {
            GetInputActionMapByType(action).performed -= callback;
        }

        public void UnregisterCanceled(Command action, Action<InputAction.CallbackContext> callback)
        {
            GetInputActionMapByType(action).canceled -= callback;
        }
        #endregion

        private InputAction GetInputActionMapByType(Command action)
        {
            return Gameplay.FindAction(action.ToString());
        }

        // private void OnChangeChange(GameScene gameScene)
        // {
        //     if (gameScene is GameScene.WorldLevel_ or GameScene.WorldMap)
        //     {
        //         EnableGameplayInput();
        //     }
        //     else if (Gameplay.enabled)
        //     {
        //         DisableGameplayInput();
        //     }
        // }
    }
}