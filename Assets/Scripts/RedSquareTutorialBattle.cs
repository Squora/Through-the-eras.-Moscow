using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(AudioSource))]
public class RedSquareTutorialBattle : MonoBehaviour
{
    [SerializeField] private GameObject _lightEnemyPrefab;
    [SerializeField] private GameObject _heavyEnemyPrefab;
    [SerializeField] private GameObject _shooterEnemyPrefab;
    [SerializeField] private GameObject _cuirassierEnemyPrefab;

    //[SerializeField] private GameObject[] _aliveEnemies;
    [SerializeField] private List<GameObject> _aliveEnemies;
    [SerializeField] private int _currentBattle;
    [SerializeField] private int _pointSpawnRadius = 5;

    public bool FirstBattleWasStarted = false;
    public bool SecondBattleWasStarted = false;
    public bool ThirdBattleWasStarted = false;
    public bool FourthBattleWasStarted = false;
    public bool FifthBattleWasStarted = false;
    public bool FinalBattleWasStarted = false;
    public bool ReinforcementWasStarted = false;

    public bool FirstMessageWasShown = false;
    public bool SecondMessageWasShown = false;
    public bool ThirdMessageWasShown = false;
    public bool FourthMessageWasShown = false;
    public bool FifthMessageWasShown = false;
    public bool FinalMessageWasShown = false;

    [SerializeField] private float _firstBattleDelay = 2f;
    [SerializeField] private float _secondBattleDelay = 2f;
    [SerializeField] private float _thirdBattleDelay = 2f;
    [SerializeField] private float _fourthBattleDelay = 2f;
    [SerializeField] private float _fifthBattleDelay = 2f;
    [SerializeField] private float _finalBattleDelay = 2f;
    [SerializeField] private float _reinforcementDelay = 2f;

    private AIAssistant _aiAssistant;
    [SerializeField] private GameObject _endPanel;
    //[Header("Audio parameters")]
    //[SerializeField] private AudioClip _audioClip;
    //private AudioSource _audioSource;

    private void Start()
    {
        _aiAssistant = FindObjectOfType<AIAssistant>();
        //_audioSource = GetComponent<AudioSource>();
        _currentBattle = 3;
        _aiAssistant.SayInformation("����� ��������� ������, ��������������" +
            "�������� [E]", 5f);
    }

    private void Update()
    {
        _aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        if (_aliveEnemies.Count > 0)
        {
            foreach (var enemy in _aliveEnemies.ToList())
            {
                if (!enemy.GetComponent<Health>().IsAlive)
                {
                    _aliveEnemies.Remove(enemy);
                }
            }
        }

        if (_aliveEnemies.Count == 0 && _currentBattle == 5)
        {
            if (!FirstMessageWasShown)
            {
                GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                    analogMovement = true;
                _aiAssistant.SayInformation("��: �� ���������� ������ � ������ 1812 " +
                    "����!\r\n\r\n����������, �� � ���������.\r\n\r\n��� ����� �����" +
                    " ��� ������� �������� � ��������� �������� ����������. �� ���� " +
                    "������� ������������ �� �������.\r\n\r\n��������!", 10);
                FirstMessageWasShown = true;
            }
        }
        else if (_aliveEnemies.Count == 0 && _currentBattle == 4)
        {
            if (!SecondMessageWasShown)
            {
                GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                    analogMovement = true;
                _aiAssistant.SayInformation("��: ����, �� ���� ������ ������������ " +
                    "��������� ����� �����������.\r\n������� �������: � ��� ��� ������ " +
                    "� ������ ������������.\r\n������: ������ ����� ��� ��� ���� �������" +
                    " � ����������� ������������\r\n\r\n������������ ����� � ������ ��� " +
                    "���������, ���� ��� ���� ����� � ���������� ������!\r\n\r\n��������!",
                    10);
                SecondMessageWasShown = true;
            }
        }
        else if (_aliveEnemies.Count == 0 && _currentBattle == 2)
        {
            if (!ThirdMessageWasShown)
            {
                _aiAssistant.SayInformation("�� �������: ����������� ��������� " +
                    "��������������� ��������.\r\n�����: 1,3 ��.\r\n������: 17.1 ��" +
                    "\r\n������� ���������� � ������� ���� �������� ���������� �����, " +
                    "����� ���������� ����������.\r\n���������� ���������: ������ 50 " +
                    "������ �������� ������������.\r\n\r\n�������������: �� ����������� " +
                    "������� ������ ��� ����������� ����� ������ � ������� � ���������� " +
                    "������������ ��� � �������� ������������� � �� �������������� � " +
                    "���� ���. � ����������� �� ����� � ���������� ����������� � �� " +
                    "����� ����������� ��� ����� ����� �� 1 �� 3 ���.\r\n\r\n����������" +
                    " � ������� R\r\n����������� � 15 ������ (����)", 10f);
                ThirdMessageWasShown = true;
            }
        }
        else if (_currentBattle == 6)
        {
            if (!FourthMessageWasShown)
            {
                _aiAssistant.SayInformation("��: � ��� ��� ������ � ������ ������������:" +
                    "\r\n\r\n�������� �� ���� ��� �������� ����\r\n����������� " +
                    "��������� �������� �����������\r\n\r\n������� [T]", 5f);
                Time.timeScale = 0.1f;
                Invoke("UnFreezeTime", 1f);
                FourthMessageWasShown = true;
            }
        }
        else if (_currentBattle == 6 && FourthMessageWasShown)
        {
            if (!FifthMessageWasShown)
            {
                _aiAssistant.SayInformation("��: �������� ������ ����������� � " +
                    "���������� �������� �����������, ����� ������ ����� � ����\r\n\r\n" +
                    "������� [K] ����������� ������ �����������", 5f);
                Time.timeScale = 0.1f;
                Invoke("UnFreezeTime", 1f);
                FourthMessageWasShown = true;
            }
        }

        if (FinalBattleWasStarted && _aliveEnemies.Count == 0)
        {
            if (!FinalMessageWasShown)
            {
                _endPanel.GetComponent<Animator>().Play("SmoothlyTransition");
                _aiAssistant.SayInformation("������ �������� � ���������� ������! " +
                    "(����� ���������������� ������)", 15f);
                Invoke("QuitApplication", 5f);
                FinalMessageWasShown = true;
            }
        }

        switch (_currentBattle)
        {
            case 1:
                if (_aliveEnemies.Count == 0) Invoke("FirstBattle", _firstBattleDelay);
                break;
            case 2:
                if (_aliveEnemies.Count == 0) Invoke("SecondBattle", _secondBattleDelay);
                break;
            case 3:
                if (_aliveEnemies.Count == 0) Invoke("ThirdBattle", _thirdBattleDelay); 
                break;
            case 4:
                if (_aliveEnemies.Count == 0) Invoke("FourthBattle", _fourthBattleDelay);
                break;
            case 5:
                if (_aliveEnemies.Count == 0) Invoke("Reinforcement", _reinforcementDelay);
                break;
            case 6:
                if (_aliveEnemies.Count == 0) Invoke("FifthBattle", _fifthBattleDelay);
                break;
            case 7:
                if (_aliveEnemies.Count == 0) Invoke("FinalBattle", _finalBattleDelay);
                break;
        }
    }

    private void UnFreezeTime() { Time.timeScale = 1.0f; }

    private void FirstBattle()
    {
        if (!FirstBattleWasStarted)
        {
            SpawnEnemy(_lightEnemyPrefab);
            _currentBattle++;
            FirstBattleWasStarted = true;
        }
    }

    private void SecondBattle()
    {
        if (!SecondBattleWasStarted)
        {
            SpawnEnemy(_heavyEnemyPrefab);
            SpawnEnemy(_heavyEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            _currentBattle++;
            SecondBattleWasStarted = true;
        }
    }

    private void ThirdBattle()
    {
        if (!ThirdBattleWasStarted)
        {
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_shooterEnemyPrefab);
            _currentBattle++;
            ThirdBattleWasStarted = true;
        }
    }

    private void FourthBattle()
    {
        if (!FourthBattleWasStarted)
        {
            //_audioSource.Play();
            GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = false;
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_heavyEnemyPrefab);
            SpawnEnemy(_heavyEnemyPrefab);
            _currentBattle++;
            FourthBattleWasStarted = true;
        }
    }

    private void FinalBattle()
    {
        if(!FinalBattleWasStarted)
        {
            _aiAssistant.SayInformation(
                "��: � ��� ��� ������ � �������� �����������, ��� " +
                "������������� ��������� ������ ����� � ������� ���� " +
                "������. �� � �� ���� �������, ����������� � �������!", 10);
            SpawnEnemy(_cuirassierEnemyPrefab);
            SpawnEnemy(_cuirassierEnemyPrefab);
            _currentBattle++;
            Invoke("FinalBattleStrikers", 10f);
            FinalBattleWasStarted = true;
        }
    }

    private void FinalBattleStrikers()
    {
        for (int i = 0; i < 8; i++)
        {
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_shooterEnemyPrefab);
        }
    }

    private void Reinforcement()
    {
        if (!ReinforcementWasStarted)
        {
            GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = false;
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            _currentBattle++;
            ReinforcementWasStarted = true;
        }
    }

    private void FifthBattle()
    {
        if (!FifthBattleWasStarted)
        {
            //_audioSource.Play();
            GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = false;
            Debug.Log("Fifthg");
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_shooterEnemyPrefab);
            SpawnEnemy(_shooterEnemyPrefab);

            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_lightEnemyPrefab);
            SpawnEnemy(_heavyEnemyPrefab);
            _currentBattle++;
            FifthBattleWasStarted = true;
        }
    }

    private Vector3 GetRandomPointInRadius()
    {
        Vector2 randomPoint = Random.insideUnitCircle * _pointSpawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
        return spawnPosition;
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, GetRandomPointInRadius(), Quaternion.identity);
    }

    private void QuitApplication() { Application.Quit(); }
}
