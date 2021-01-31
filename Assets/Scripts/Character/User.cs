using LonelyIsland.Misc;
using LonelyIsland.System;
using LonelyIsland.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LonelyIsland.Characters
{
    public class User : Character
    {
        [Header("Camera")]
        [SerializeField] private Camera characterCamera;
        [Header("Input")]
        public PlayerControls controls;
        [SerializeField] private CharacterController charController;

        [SerializeField] private Transform SpawnPoint;
        [SerializeField] private MainMenu mainMenu;

        public override float Damage { get { return GameManager.Instance.Stats.Damage * DamageMultiplier; } }
        public override float TotalMaxHealth { get { return GameManager.Instance.Stats.Health * HealthMultiplier; } }
        public override float TotalMaxDamage { get { return GameManager.Instance.Stats.Damage * DamageMultiplier; } }

        protected override void Awake()
        {
            base.Awake();

            InitControls();

            if (characterCamera == null)
                characterCamera = Camera.main;

            if (GameManager.Instance == null) return;

            float newHealth = GameManager.Instance.Save.Health == -1 ? TotalMaxHealth : GameManager.Instance.Save.Health;
            SetHealth(newHealth);

            Vector3 newPosition = GameManager.Instance.Save.Position != null ?
                GameManager.Instance.Save.Position.Vector3 :
                transform.position;

            if (GameManager.Instance.Teleporting)
            {
                GameManager.Instance.Teleporting = false;
                newPosition = SpawnPoint.position;
            }

            transform.position = newPosition;
        }

        private void JumpButtonPerformed(InputAction.CallbackContext obj)
        {
            if (charController.isGrounded)
            {
                charController.Move(new Vector3(0, JumpForce, 0));
            }
        }

        protected override float SetHealth(float newHealth) { 
            health = Mathf.Clamp(newHealth, HealthMin, TotalMaxHealth);
            GameManager.Instance.Save.Health = health;
            return health;
        }

        protected override void Update()
        {
            base.Update();

            if (controls == null)
                InitControls();

            if (globalCooldownPeriod > 0)
            {
                globalCooldownPeriod -= Time.deltaTime;
            }

            if (charController.isGrounded && Velocity.y < 0)
            {
                Velocity.y = 0;
            }

            Vector2 inputVector = controls.Player.Movement.ReadValue<Vector2>();
            float pressedJump = controls.Player.Jump.ReadValue<float>();
            float sprint = controls.Player.Sprint.ReadValue<float>();
            Sprint = sprint == 1;

            float movementSpeed = IsSprinting ? MovementSprintSpeed : MovementSpeed;
            Vector3 targetVector = new Vector3(inputVector.x, 0, inputVector.y);

            if (charController.isGrounded && pressedJump != 0)
                Velocity.y += Mathf.Sqrt(JumpForce * -3.0f * Gravity);

            Velocity.y += Gravity * Time.deltaTime;

            Vector3 movementVector = MoveToTarget(targetVector * movementSpeed);
            charController.Move(Velocity * Time.deltaTime);
            RotateTowardsTarget(movementVector);
        }

        private void LateUpdate()
        {
            if (GameManager.Instance == null) return;

            GameManager.Instance.Save.Health = health;
            GameManager.Instance.Save.Position = new SerializedVector3(transform.position);
        }

        private void RotateTowardsTarget(Vector3 movementVector)
        {
            Vector3 look = new Vector3(movementVector.x, 0, movementVector.z);
            if (look.magnitude == 0) return;
            Quaternion rotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.RotateTowards(transform.rotation, rotation, 360),
                IsSprinting ? RotateSprintSpeed : RotateSpeed
            );
        }

        private Vector3 MoveToTarget(Vector3 targetVector)
        {
            targetVector = Quaternion.Euler(0, characterCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
            charController.Move(targetVector * Time.deltaTime);
            return targetVector;
        }

        private void OnEnable()
        {
            if (controls != null)
                controls.Enable();
            else InitControls();
        }

        private void OnDisable()
        {
            if (controls != null)
                controls.Disable();
        }

        private void InitControls()
        {
            controls = new PlayerControls();
            controls.Player.Attack.performed += AttackButtonPressed;
            controls.Player.Jump.performed += JumpButtonPerformed;
            controls.Player.ToggleSpring.performed += _ => ToggleSpring = !ToggleSpring;
        }

        private void AttackButtonPressed(InputAction.CallbackContext obj)
        {
            if (globalCooldownPeriod > 0) return;
            animationController.SetTrigger("Attack");
            globalCooldownPeriod = GlobalCooldown;
        }

        protected override void Died()
        {
            mainMenu.gameObject.SetActive(true);
            base.Died();
        }
    }
}
