using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Игрок")]
    [SerializeField] Player player;
    [SerializeField, Min(0)] float tabooRadius;
    [SerializeField, Range(5, 20)] float maxInstatiateRadius;

    [Header("Префабы")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bonusPrefab;

    [Header("Данные")]
    [SerializeField, Range(3, 100)] int maxEnemyCount;
    [SerializeField, Range(3, 100)] int maxBonusCount;
    [SerializeField, Range(0, 10)] float enemyRespawnTime;
    [SerializeField, Range(0, 10)] float bonusRespawnTime;

    public int enemyesCount;
    public int bonusesCount;

    float _enemyCurrentRespawnTime = 0f;
    float _bonusCurrentRespawnTime = 0f;
    bool _gamePlay;

    #region Start Update
    private void Start()
    { 
    }
    void Update()
    {
        RespawnGameObjects(ref enemyesCount, maxEnemyCount, enemyRespawnTime, ref _enemyCurrentRespawnTime, enemyPrefab);
        RespawnGameObjects(ref bonusesCount, maxBonusCount, bonusRespawnTime, ref _bonusCurrentRespawnTime, bonusPrefab);
    }
    #endregion

    #region CreatePrefab
    void RespawnGameObjects(ref int count, int maxCount, float respawnTime, ref float currentRespawnTime, GameObject prefab)
    {
        if (count > maxCount) return;

        if (currentRespawnTime < respawnTime)
        {
            currentRespawnTime += Time.deltaTime;
        }
        else
        {
            currentRespawnTime = 0f;
            CreatePrefab(prefab, ref count);
        }
    }

    void CreatePrefab(GameObject prefab, ref int countPrefabs)
    {
        Instantiate(prefab, FindInstatiatePosition(), Quaternion.identity);
        countPrefabs++;
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
        if (_gamePlay) return;

        _gamePlay = false;

    }

    public void NewGame()
    {
        _gamePlay = true;

        _enemyCurrentRespawnTime = 0f;
        _bonusCurrentRespawnTime = 0f;

        CreatePrefab(enemyPrefab, ref enemyesCount);
    }
    #endregion
}
