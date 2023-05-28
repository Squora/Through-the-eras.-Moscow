using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    [SerializeField] private float _firstBattleDelay = 2f;
    [SerializeField] private float _secondBattleDelay = 2f;
    [SerializeField] private float _thirdBattleDelay = 2f;
    [SerializeField] private float _fourthBattleDelay = 2f;
    [SerializeField] private float _fifthBattleDelay = 2f;
    [SerializeField] private float _finalBattleDelay = 2f;
    [SerializeField] private float _reinforcementDelay = 2f;

    private void Start()
    {
        _currentBattle = 1;
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
           GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = true;
            FindObjectOfType<AIAssistant>().SayInformation(
                "ИИ: Мы однозначно попали в Москву 1812 года!\r\n\r\nПоздравляю," +
                " вы — попаданец.\r\n\r\nМне нужно время для анализа ситуации и " +
                "активации защитных протоколов. До того времени постарайтесь не " +
                "умирать.\r\n\r\nВнимание!", 10);
        }
        else if (_aliveEnemies.Count == 0 && _currentBattle == 4)
        {
            GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = true;
            FindObjectOfType<AIAssistant>().SayInformation(
                "ИИ: Итак, по моим данных приближается последняя волна противников." +
                " \r\nХорошая новость: я даю вам доступ к особым способностям." +
                "\r\nПлохая: скорее всего они вам мало помогут с сильнейшими " +
                "противниками\r\n\r\nПродержитесь здесь и помогу вам выбраться, " +
                "ведь наш путь лежит в Подземелья Кремля!\r\n\r\nВнимание!", 10);
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
        if(FinalBattleWasStarted)
        {
            FindObjectOfType<AIAssistant>().SayInformation(
                "ИИ: я даю вам доступ к защитной способности, она " +
                "предотвращает получение любого урона в течении пяти " +
                "секунд. Но у неё мало зарядов, используйте её разумно!", 10);
            SpawnEnemy(_cuirassierEnemyPrefab);
            SpawnEnemy(_cuirassierEnemyPrefab);
            _currentBattle++;
            FinalBattleWasStarted = true;
        }
    }

    private void Reinforcement()
    {
        if (!ReinforcementWasStarted)
        {
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
            GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>().
                analogMovement = false;
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
}
