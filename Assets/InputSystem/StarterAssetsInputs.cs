using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 sprint;
        public Vector2 look;
        public bool move;
        public bool dash;
        public bool fixCursorOnEnemy;
        public bool interact;
        public bool attack;
        public bool comboAttack;
        public bool shootPistol;
        public bool canShoot = true;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        ThirdPersonController player = new ThirdPersonController();

        private void Start()
        {
            player = FindObjectOfType<ThirdPersonController>();
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.isPressed);
        }

        public void OnDash(InputValue value)
        {
            DashInput(value.isPressed);
        }

        public void OnFixCursorOnEnemy(InputValue value)
        {
            FixCursorOnEnemyInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnAttack(InputValue value)
        {
            if (player.PlayerDetected)
            {
                AttackInput(value.isPressed);
            }
            //AttackInput(value.isPressed);
        }

        public void OnComboAttack(InputValue value)
        {
            if (player.PlayerDetected)
            {
                ComboAttackInput(value.isPressed);
            }
        }

        public void OnShootPistol(InputValue value)
        {
            if (canShoot && player.PlayerDetected)
            {
                ShootPistolInput(value.isPressed);
            }
        }

        public void MoveInput(bool newMoveState)
        {
            move = newMoveState;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void SprintInput(Vector2 newSprintDirection)
        {
            sprint = newSprintDirection;
        }

        public void DashInput(bool newDashState)
        {
            dash = newDashState;
        }

        public void ShootPistolInput(bool newShootState)
        {
            shootPistol = newShootState;
        }

        public void FixCursorOnEnemyInput(bool newFixCursorOnEnemy)
        {
            if (player.PlayerDetected)
            {
                if (!fixCursorOnEnemy)
                {
                    fixCursorOnEnemy = newFixCursorOnEnemy;
                }
                else
                {
                    fixCursorOnEnemy = false;
                }
            }
        }
        public void InteractInput(bool newInteractState)
        {
            //if (!interact)
            //{
            //    interact = newInteractState;
            //}
            //else
            //{
            //    interact = false;
            //}
            interact = newInteractState;
        }

        public void AttackInput(bool newAttackState)
        {
            attack = newAttackState;
        }

        public void ComboAttackInput(bool newComboAttackState)
        {
            comboAttack = newComboAttackState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}