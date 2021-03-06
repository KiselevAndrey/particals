using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public ParticleSystem particle;
    [SerializeField] Collider2D colider;
    [SerializeField, Min(0)] float lifeTime;

    GameManager _gameManager;
    float _timeLived;
    float _startedScale;

    #region Awake Update
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        //_startedScale = transform.localScale.x;
    }

    private void Update()
    {
        if (_timeLived >= lifeTime) Die();

        _timeLived += Time.deltaTime;

        //float percentLived = _timeLived * 100 / lifeTime;
        //float multiplayScale = percentLived * _startedScale / 100;

        //print(multiplayScale);
        //transform.localScale = Vector3.one * multiplayScale;
        //particle.transform.localScale = Vector3.one * multiplayScale;
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Enemy:
                Enemy enemy = collision.GetComponent<Enemy>();
                AddEnemyScale(enemy);
                break;

            case TagsNames.Player:
                Player player = collision.GetComponent<Player>();
                player.AddTrap();
                break;
        }

        Die();
    }
    #endregion

    #region ChangeScale
    void AddEnemyScale(Enemy enemy)
    {
        Vector3 temp = Vector3.one;

        enemy.transform.localScale += temp;
        enemy.particle.transform.localScale += temp;
    }
    #endregion

    #region Die
    void Die()
    {
        colider.enabled = false;
        _gameManager.bonusesCount--;
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
}
