using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ParticleSystem particle;
    [SerializeField, Range(0f, 0.01f)] float modifier;
    public Collider2D colider;

    [HideInInspector] public bool isGrow;
    [HideInInspector] public bool isChangedGrow;
    [HideInInspector] public Folower2D folower;

    GameManager _gameManager;
    bool _isDie;

    #region Awake FixedUpdate
    private void Awake()
    {
        folower = GetComponent<Folower2D>();
    }

    void FixedUpdate()
    {
        folower.speed += modifier;
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Enemy:
                Enemy enemy = collision.GetComponent<Enemy>();
                CheckIsGrow(enemy);
                break;

            case TagsNames.Player:
                _gameManager.GameOver();
                Die();
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Enemy:
                Enemy enemy = collision.GetComponent<Enemy>();
                ChangeScale(enemy);
                CheckDie();
                break;
        }
    }
    #endregion

    #region ChangeScale
    /// <summary>
    /// у кого больше Scale тот и растет
    /// </summary>
    void CheckIsGrow(Enemy enemy)
    {
        isGrow = transform.localScale.magnitude >= enemy.transform.localScale.magnitude;
        enemy.isGrow = !isGrow;
        isChangedGrow = false;
    }

    void ChangeScale(Enemy enemy)
    {
        if (isChangedGrow) return;

        Vector3 temp = transform.localScale * (enemy.isGrow ? 1 : -1);

        enemy.transform.localScale += temp;
        transform.localScale -= temp;

        enemy.particle.transform.localScale += temp;
        particle.transform.localScale -= temp;

        enemy.isChangedGrow = isChangedGrow = true;
    }

    #endregion

    #region Die
    void CheckDie()
    {
        if (transform.localScale.magnitude < 0.1f)
            Die();
    }

    public void Die()
    {
        if (_isDie) return;

        _isDie = true;
        colider.enabled = false;
        _gameManager.enemyesCount--;
        folower.speed = 0;
        modifier = 0;
        StartCoroutine(DestroyMe());
    }

    IEnumerator DestroyMe()
    {
        while (transform.localScale.magnitude > 0.1f)
        {
            Vector3 temp = transform.localScale * 0.1f;

            transform.localScale -= temp;
            particle.transform.localScale -= temp;

            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
    #endregion

    #region SetGameManager
    public void SetGameManager(GameManager gm) => _gameManager = gm;
    #endregion
}
