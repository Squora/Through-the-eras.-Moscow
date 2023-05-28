using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]

    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;
        public AudioClip[] FootstepWalkAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
        [Space(10)]
        public float Gravity = -15.0f;
        [Space(10)]
        public float FallTimeout = 0.15f;
        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;
        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _fallTimeoutDelta;

        private PlayerInput _playerInput;
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private AudioSource _audioSource;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        public bool PlayerDetected = false;
        [SerializeField] private GameObject _closestEnemy;

        private bool _canDodge = true;

        [Header("Attack parameters")]
        [SerializeField] private float _hitCooldown = 2f;
        [SerializeField] private bool _canAttack = true;
        [SerializeField] private float _attackTimer;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackPointRadius;
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private LayerMask _attackMask;
        [Header("Shoot ability parameters")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletSpawn;
        [SerializeField] private GameObject _pistol;
        [SerializeField] private int _shootForce = 20;
        [SerializeField] private int _shootDamage = 10;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return _playerInput.currentControlScheme == "KeyboardMouse";
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();
            _audioSource = GetComponent<AudioSource>();

            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            FindClosestEnemy();
            FixCursorOnTarget(_closestEnemy);
            JumpAndGravity();
            GroundedCheck();

            if (!_input.attack)
            {
                Move();
            }
            Attack();
            ComboAttack();
            AttackCooldown();
            ShootPistol();

            //Enemy[] enemies = FindObjectsOfType<Enemy>();
            //if (enemies != null)
            //{
            //    foreach (var enemy in enemies)
            //    {
            //        enemy.OnPlayerSpotted += OnPlayerSpotted;
            //        enemy.OnPlayerMissing += OnPlayerMissing;
            //    }
            //}
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        //private void OnPlayerSpotted()
        //{
        //    Debug.Log("Enemy detected the player!");
        //    PlayerDetected = true;
        //    _closestEnemy = FindClosestEnemy();
        //}

        //private void OnPlayerMissing()
        //{
        //    Debug.Log("Enemy missed the player!");
        //    PlayerDetected = false;
        //    _input.fixCursorOnEnemy = false;
        //}

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y
                - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            if (_input.fixCursorOnEnemy == false)            {
                // if there is an input and camera position is not fixed
                if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
                {
                    //Don't multiply mouse input by Time.deltaTime;
                    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                    _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                    _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
                }

                // clamp our rotations so our values are limited 360 degrees
                _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Cinemachine will follow this target
                CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);
            }
        }

        private void FixCursorOnTarget(GameObject target)
        {
            if (_closestEnemy != null && _input.fixCursorOnEnemy)
            {
                Vector3 direction = target.transform.position - transform.position;
                //transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                CinemachineCameraTarget.transform.rotation = Quaternion.LookRotation
                    (new Vector3(direction.x, -2, direction.z));
            }
        }

        public void FindClosestEnemy()
        {
            if (_closestEnemy != null)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(enemy.transform.position, gameObject.transform.position)
                        < Vector3.Distance(_closestEnemy.transform.position, gameObject.transform.position))
                    {
                        _closestEnemy = enemy;
                    }
                }
            }
        }

        private void Move()
        {
            float targetSpeed = _input.move ? MoveSpeed : SprintSpeed;

            if (_input.analogMovement) SprintSpeed = MoveSpeed;
            else SprintSpeed = 5.335f;

            if (_input.sprint == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.sprint.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.sprint.x, 0.0f, _input.sprint.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.sprint != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat("Speed", _animationBlend);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        //private IEnumerator Dodge()
        //{
        //    if (_input.dash && _canDodge)
        //    {
        //        _canDodge = false;
        //        Debug.Log("Work");
        //        Vector3 direction = _input.sprint;
        //        Vector3 dodgeDirection = -transform.forward;
        //        _controller.Move(direction * 100 * Time.fixedDeltaTime);
        //        yield return new WaitForSeconds(3);
        //        _canDodge = true;
        //    }
        //}

        private void Attack()
        {
            if (_input.attack && _canAttack)
            {
                transform.LookAt(_closestEnemy.transform);
                _animator.SetTrigger("Attack");
                _canAttack = false;

                _colliders = Physics.OverlapSphere(_attackPoint.position,
                    _attackPointRadius, _attackMask);

                foreach (var collider in _colliders)
                {
                    if (collider.tag == "Enemy" && Vector2.Distance(collider.gameObject.
                        transform.position, gameObject.transform.position) < 2)
                    {
                        collider.GetComponent<Health>().TakeDamage(10);
                    }
                }
            }
        }

        private void ComboAttack()
        {
            if (_input.comboAttack)
            {
                transform.LookAt(_closestEnemy.transform);
                _animator.SetTrigger("ComboAttack");
                _canAttack = false;

                _colliders = Physics.OverlapSphere(_attackPoint.position,
                    _attackPointRadius, _attackMask);

                foreach (var collider in _colliders)
                {
                    if (collider.tag == "Enemy" && Vector2.Distance(collider.gameObject.
                        transform.position, gameObject.transform.position) < 2)
                    {
                        collider.GetComponent<Health>().TakeDamage(30);
                    }
                }
            }
        }

        private void AttackCooldown()
        {
            if (!_canAttack)
            {
                if (_attackTimer < _hitCooldown)
                {
                    _attackTimer += Time.deltaTime;
                }
                else
                {
                    _attackTimer = 0;
                    _canAttack = true;
                }
            }
        }

        private void ShootPistol()
        {
            if (_input.shootPistol && _input.canShoot)
            {
                _input.shootPistol = false;
                StartCoroutine(SetPistolActive(true, 0.2f));
                _animator.SetTrigger("ShootPistol");
                Invoke("MakeShot", 1f);
                transform.LookAt(_closestEnemy.transform.position);
                StartCoroutine(SetPistolActive(false, 2f));
                _input.canShoot = false;
                StartCoroutine(Reload(10));
            }
        }

        private void MakeShot()
        {
            Vector3 direction = _closestEnemy.transform.position
                - transform.position;
            GameObject bullet = Instantiate(_bulletPrefab,
                _bulletSpawn.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce
                (direction.normalized * _shootForce, ForceMode.Impulse);
            _closestEnemy.GetComponent<Health>().TakeDamage(_shootDamage);
            _audioSource.Play();
        }

        private IEnumerator SetPistolActive(bool value, float delay)
        {
            yield return new WaitForSeconds(delay);
            _pistol.SetActive(value);
        }

        private IEnumerator Reload(float delay)
        {
            yield return new WaitForSeconds(delay);
            _input.canShoot = true;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnWalkFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepWalkAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepWalkAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepWalkAudioClips[index], 
                        transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy") _closestEnemy = other.gameObject; PlayerDetected = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Enemy") PlayerDetected = false;
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackPointRadius);
        }
    }
}