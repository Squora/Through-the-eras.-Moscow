using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] private float _walkSpeed = 3.0f;
    [SerializeField] private float _waitTime = 5f;
    public NavMeshAgent Agent;
    private float _timer = 0;
    [SerializeField] public MovementType _movementType = MovementType.None;
    public MovementType _previousMovementType;
    [Header("Holding parameters")]
    [SerializeField] private Transform _holdPoint;
    [Header("Patrolling parameters")]
    [SerializeField] private Transform[] _goals;
    [SerializeField] private float _distanceToChangeGoalOfPatrolling = 0.5f;
    [SerializeField] private int _currentGoal = 0;
    [Header("FreeWalk parameters")]
    [SerializeField] private float _pointSpawnRadius;
    [SerializeField] private GameObject _pointToMove;
    [SerializeField] private float _distaneToChangeGoalOfWalking = 0.5f;
    [Header("Follow player parameters")]
    [SerializeField] private float _followSpeed = 6.0f;
    [SerializeField] private float _stoppingDistance = 2f;
    [SerializeField] private string _detectionTag = "Player";
    public GameObject Player;
    public bool PlayerInArea = false;
    [Header("Gizmos parameters")]
    [SerializeField] private bool _isShowing = true;
    [SerializeField] private Color _spawnRadiusColor = Color.white;

    [SerializeField] private AudioClip[] _voiceLines;
    public delegate void PlayerDetectedEventHandler();
    public event PlayerDetectedEventHandler OnPlayerSpotted, OnPlayerMissing;
    private Animator _animator;

    public bool PlayerIsOnAttackDistantion = false;
    //[Header("Attack parameters")]
    //[SerializeField] private int _damage = 10;
    //[SerializeField] private float _hitCooldown = 2f;
    //[SerializeField] private bool _canAttack = true;
    //[SerializeField] private float _attackTimer;

    public enum MovementType
    {
        None,
        Hold,
        Patrol,
        FreeWalk,
        FollowPlayer
    }

    void Start()
    {
        int randomVoiceLine = Random.Range(0, _voiceLines.Length);
        GetComponent<AudioSource>().clip = _voiceLines[randomVoiceLine];
        GetComponent<AudioSource>().Play();
        Agent = GetComponent<NavMeshAgent>();
        //_player = GameObject.FindGameObjectWithTag("Player").transform;
        Agent.speed = _walkSpeed;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //ResetAttackCooldown();
    }

    void FixedUpdate()
    {
        Move();
    }

#region[Header(Movement)]

    private void Move()
    {
        switch (_movementType)
        {
            case MovementType.None:
                break;
            case MovementType.Hold:
                Hold();
                break;
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.FreeWalk:
                FreeWalk();
                break;
            case MovementType.FollowPlayer:
                FollowPlayer();
                break;
        }
    }

    private void Hold()
    {
        _animator.SetFloat("Speed", 0);
        if (transform.position != _holdPoint.position)
        {
            _animator.SetFloat("Speed", _walkSpeed);
            Agent.SetDestination(_holdPoint.position);
        }
    }

    void Patrol()
    {
        if (Agent.remainingDistance < _distanceToChangeGoalOfPatrolling)
        {
            if (_timer <= 0)
            {
                _currentGoal++;
                if (_currentGoal == _goals.Length) _currentGoal = 0;
                Agent.destination = _goals[_currentGoal].position;
                _timer = _waitTime;
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }

    void FollowPlayer()
    {
        transform.LookAt(Player.transform);
        if (Vector3.Distance(gameObject.transform.position, Player.transform.position) > 
            _stoppingDistance/* && Agent.isStopped == false*/)
        {
            PlayerIsOnAttackDistantion = false;
            _animator.SetFloat("Speed", _followSpeed);
            Agent.SetDestination(Player.transform.position);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
            PlayerIsOnAttackDistantion = true;
            //GetComponent<MeleeCombat>().Attack();
        }
    }

    private void FreeWalk()
    {
        if (_pointToMove == null)
        {
            _pointToMove = new GameObject("PointToMove");
            _pointToMove.transform.position = GetRandomPointInRadius();
        }
        else
        {
            _timer += Time.deltaTime;
            Agent.SetDestination(_pointToMove.transform.position);
            if (_timer > _waitTime)
            {
                Destroy(_pointToMove);
                _timer = 0;
            }
        }
    }

    private Vector3 GetRandomPointInRadius()
    {
        Vector2 randomPoint = Random.insideUnitCircle * _pointSpawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
        return spawnPosition;
    }

    #endregion

//#region[Header(Attack)]

//    private void Attack()
//    {
//        if (_canAttack)
//        {
//            _animator.SetTrigger("Attack");
//            _canAttack = false;
//            _player.GetComponent<PlayerHealth>().TakeDamage(_damage);
//        }
//    }

//    private void ResetAttackCooldown()
//    {
//        if (!_canAttack)
//        {
//            if (_attackTimer < _hitCooldown)
//            {
//                _agent.isStopped = true;
//                _attackTimer += Time.deltaTime;
//            }
//            else
//            {
//                _agent.isStopped = false;
//                _attackTimer = 0;
//                _canAttack = true;
//            }
//        }
//    }

//#endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_detectionTag))
        {
            OnPlayerSpotted?.Invoke();
            PlayerInArea = true;
            Player = other.gameObject;
            _previousMovementType = _movementType;
            _movementType = MovementType.FollowPlayer;
            Agent.speed = _followSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_detectionTag))
        {
            OnPlayerMissing?.Invoke();
            PlayerInArea = false;
            //Player = null;
            _movementType = _previousMovementType;
            Agent.speed = _walkSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_isShowing)
        {
            Gizmos.color = _spawnRadiusColor;
            Gizmos.DrawWireSphere(transform.position, _pointSpawnRadius);
        }
    }
}
