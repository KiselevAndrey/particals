using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Игрок")]
    [SerializeField] Player player;
    [SerializeField, Min(0)] float tabooRadius;
    [SerializeField, Range(5, 20)] float maxInstatiateRadius;

    [Header("Префабы")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bonusPrefab;

    [Header("Данные Enemy")]
    [SerializeField, Range(3, 100)] int enemyMaxCount;
    [SerializeField, Range(0, 10)] float enemyRespawnTime;
    [SerializeField, Range(0, 100)] float enemyChanceUpdateRespawn;

    [Header("Данные Bonus")]
    [SerializeField, Range(3, 100)] int bonusMaxCount;
    [SerializeField, Range(0, 10)] float bonusRespawnTime;
    [SerializeField, Range(0, 100)] float bonusChanceUpdateRespawn;

    [Header("Данные доп")]
    [SerializeField] TimerUI timerGame;
    [SerializeField] TimerUI timerMenu;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject game;
    [SerializeField] Button btnStart;
    [SerializeField] GameStatSO gameStats;

    [HideInInspector] public int enemyesCount;
    [HideInInspector] public int bonusesCount;

    float _enemyRespawnTime;
    float _bonusRespawnTime;
    float _enemyCurrentRespawnTime = 0f;
    float _bonusCurrentRespawnTime = 0f;
    float _enemyChanceUpdateRespawn;
    float _bonusChanceUpdateRespawn;

    bool _gamePlay = true;

    #region Start Update
    private void Start()
    {
        GameOver();

        if (!gameStats.tutorialEnding) btnStart.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        RespawnGameObjects(ref enemyesCount, enemyMaxCount, _enemyRespawnTime, ref _enemyCurrentRespawnTime, enemyPrefab);
        RespawnGameObjects(ref bonusesCount, bonusMaxCount, _bonusRespawnTime, ref _bonusCurrentRespawnTime, bonusPrefab);
        UpdateDifficult(ref _enemyRespawnTime, ref _enemyChanceUpdateRespawn);
        UpdateDifficult(ref _bonusRespawnTime, ref _bonusChanceUpdateRespawn);
    }
    #endregion

    #region CreatePrefab
    void RespawnGameObjects(ref int count, int maxCount, float respawnTime, ref float currentRespawnTime, GameObject prefab)
    {
        if (count > maxCount) return;

        if (currentRespawnTime < respawnTime)
        {
            currentRespawnTime += Time.fixedDeltaTime;
        }
        else
        {
            currentRespawnTime = 0f;
            CreatePrefab(prefab, ref count);
        }
    }

    void CreatePrefab(GameObject prefab, ref int countPrefabs)
    {
        SetGameManager(Instantiate(prefab, FindInstatiatePosition(), Quaternion.identity));
        countPrefabs++;
    }

    void SetGameManager(GameObject gameObject)
    {
        gameObject.GetComponent<Enemy>()?.SetGameManager(this);
        gameObject.GetComponent<Bonus>()?.SetGameManager(this);
    }

    Vector3 FindInstatiatePosition()
    {
        // новая позиция от центра gameManager + рандомный нормализованный вектор * рандомный радиус от 0 до мах
        Vector3 newPosition = transform.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f, transform.position.z).normalized * Random.Range(0, maxInstatiateRadius);

        // проверка на табу радиус
        if (Mathf.Abs(((Vector2)(newPosition - player.transform.position)).magnitude) < tabooRadius) return FindInstatiatePosition();
        else return newPosition;
    }
    #endregion

    #region GameOver
    public void GameOver()
    {
        if (!_gamePlay) return;

        IsGame(false);
        Reset();

        // проверить рекорд
        gameStats.CheckRecord(timerGame.GetTime());
        timerMenu.SetTime(gameStats.record);
    }

    public void NewGame()
    {
        timerGame.SetTime(0);

        IsGame(true);

        KillAll();
        Reset();

        _enemyCurrentRespawnTime = 0f;
        _bonusCurrentRespawnTime = 0f;

        CreatePrefab(enemyPrefab, ref enemyesCount);
    }

    void IsGame(bool value)
    {
        _gamePlay = value;

        player.canUpdate = value;
        player.ActiveTrap(value);

        timerGame.isTick = value;

        game.SetActive(value);
        menu.SetActive(!value);
    }

    void KillAll()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].Die();

        Bonus[] bonuses = FindObjectsOfType<Bonus>();
        for (int i = 0; i < bonuses.Length; i++)
            bonuses[i].Die();
    }

    private void Reset()
    {
        _enemyRespawnTime = enemyRespawnTime;
        _bonusRespawnTime = bonusRespawnTime;
        _enemyChanceUpdateRespawn = enemyChanceUpdateRespawn;
        _bonusChanceUpdateRespawn = bonusChanceUpdateRespawn;
    }
    #endregion

    #region UpdateDifficult
    void UpdateDifficult(ref float respawnTime, ref float chanceUpdate, float percent = 0.001f)
    {
        if (chanceUpdate > Random.value * 100)
        {
            respawnTime -= respawnTime * percent;
            respawnTime = Mathf.Max(0, respawnTime);
            chanceUpdate += percent;
        }
    }
    #endregion

    #region Доп функции
    public void ResetRecord() => gameStats.record = 0f;
    #endregion
}
