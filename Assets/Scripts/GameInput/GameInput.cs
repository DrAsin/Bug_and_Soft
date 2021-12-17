using System;
using Player;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    public class GameInput
    {
        public static PlayerControls playerControls;

        private bool firstTimeHealing;

        private float pauseCooldownSeconds = float.NegativeInfinity;

        private Action<InputAction.CallbackContext> MenuJoystickMovement;

        public void SetupInputs()
        {
            if(playerControls == null)
                playerControls = new PlayerControls();
            playerControls.Enable();

            SetupAbilitiesInput();
            SetupRollInput();
            SetupJumpInput();
            SetupInteractInput();
            SetupSwitchAbility();
            SetupStatsInput();
            SetupPlayerMovementInput();
            SetupMiscInput();
            SetupCameraInput();
            SetupPlayerAttack();
        }

        private void SetupMiscInput()
        {
            playerControls.Gameplay.PauseL.performed += ctx =>
            {
                if (ctx.action.phase == InputActionPhase.Started ||
                    ctx.action.phase == InputActionPhase.Canceled ||
                    ctx.action.phase == InputActionPhase.Disabled ||
                    ctx.action.phase == InputActionPhase.Waiting)
                {
                 
                    Debug.Log("Cant open pause menu 1");
                    return;   
                }
                
                if(GameManager.Instance.GetSceneIndex() == 4 ||
                    GameManager.Instance.GetSceneIndex() == 5 ||
                    GameManager.Instance.GetSceneIndex() == 6 ||
                    GameManager.Instance.GetMainMenuOn() || GameManager.Instance.isGameOver ||
                    GameManager.Instance.isDialogueMode || GameManager.Instance.isLevelingUp ||
                    GameManager.Instance.GetUIManager().IsShopOpened)
                    {
                        Debug.Log("Cant open pause menu 0");
                        return;
                    }
                
                Debug.Log("Pause Event Triggered");

                GameManager.Instance.isGamePaused = !GameManager.Instance.isGamePaused;
                GameManager.Instance.GetUIManager().UpdatePauseMenu();
            };

            playerControls.Gameplay.QuickSave.performed += ctx => { GameManager.Instance.QuickSave(); };

            playerControls.Gameplay.QuickLoad.performed += ctx => { GameManager.Instance.QuickLoad(); };

            playerControls.Gameplay.UseItem.performed += ctx =>
            {
                GameManager.Instance.GetInventorySlotManager().UseItem();
            };
        }

        private void SetupAbilitiesInput()
        {
            playerControls.Gameplay.SpecialMove.performed += ctx =>

            {
                if (GameManager.Instance.PlayerController != null)
                {
                    if (GameManager.Instance.PlayerController.powerUps.currentPowerUp != null)
                        GameManager.Instance.PlayerController.powerUps.currentPowerUp.Activate();
                }
            };
        }

        private void SetupRollInput()
        {
            playerControls.Gameplay.Roll.performed += ctxRoll =>
            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.Rollear();
            };
        }

        private void SetupJumpInput()
        {
            playerControls.Gameplay.Jump.performed += ctx =>
            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.Jumpear();
            };
        }

        private void SetupInteractInput()
        {
            playerControls.Gameplay.Interact.performed += ctx =>
            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.Interactear();
            };
        }

        // TEMP
        private int current = 0;

        private void SetupSwitchAbility()
        {
            playerControls.Gameplay.SwitchAbility.performed += ctx =>

            {
                if (GameManager.Instance.PlayerController != null &&
                    GameManager.Instance.PlayerController.powerUps.currentPowerUp != null)
                {
                    if (!GameManager.Instance.PlayerController.powerUps.currentPowerUp.onCooldown)
                    {
                        GameManager.Instance.PlayerController.CyclePowerUps();
                    }
                }
            };
        }

        private void SetupStatsInput()
        {
            playerControls.Gameplay.ViewStats.performed += ctx =>
            {
                GameManager.Instance.GetHUD()
                    .ShowStats();
            };

            playerControls.Gameplay.ViewStats.canceled += ctx =>
            {
                GameManager.Instance.GetHUD()
                    .HideStats();
            };
        }

        public void SetupPlayerMovementInput()
        {
            // Horizontal
            playerControls.Gameplay.Horizontal.performed += ctx =>

            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.SetHorizontalSpeed(
                        playerControls.Gameplay.Horizontal.ReadValue<float>());
            };
            playerControls.Gameplay.Horizontal.canceled += ctx =>

            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.SetHorizontalSpeed(0f);
            };

            // Vertical
            playerControls.Gameplay.Vertical.performed += ctx =>

            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.SetVerticalSpeed(
                        playerControls.Gameplay.Horizontal.ReadValue<float>());
            };
            playerControls.Gameplay.Vertical.canceled += ctx =>

            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.SetVerticalSpeed(0f);
            };
        }

        private void SetupCameraInput()
        {
            playerControls.Gameplay.Camera.performed += ctx =>
            {
                if (GameManager.Instance.GetDynamicCamera() != null)
                {
                    var value = ctx.ReadValue<Vector2>();
                    GameManager.Instance.GetDynamicCamera()
                        .UpdateOffsetJoystick(value);
                }
            };

            playerControls.Gameplay.Camera.canceled += ctx =>
            {
                if (GameManager.Instance.GetDynamicCamera() != null)
                {
                    GameManager.Instance.GetDynamicCamera()
                        .UpdateOffsetJoystick(Vector2.zero);
                }
            };
        }

        private void SetupPlayerAttack()
        {
            playerControls.Gameplay.Attack.performed += ctx =>
            {
                if (GameManager.Instance.PlayerController != null)
                    GameManager.Instance.PlayerController.combatCtrl.AttackPerformed();
            };
        }

        public void SetMenuJoystickInput(Action<InputAction.CallbackContext> action)
        {
            RemoveJoystickInput();
            playerControls.Gameplay.MenuMovement.performed += action;
        }
        
        public void RemoveJoystickInput()
        {
            playerControls.Gameplay.MenuMovement.performed -= MenuJoystickMovement;
        }

        public void DisableInput()
        {
            playerControls.Disable();
        }

        public void EnableInput()
        {
            playerControls.Enable();
        }
        void test ()
        {
            //OCTAVO INTENTO DE RUNNER
        }    
    }
}
