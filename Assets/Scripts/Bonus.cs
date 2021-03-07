using System.Collections;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public ParticleSystem particle;
    [SerializeField] Collider2D colider;
    [SerializeField, Min(0)] float lifeTime;

    GameManager _gameManager;
    float _timeLived;
    bool _isDie;

    #region Update
    private void Update()
    {
        if (_timeLived >= lifeTime) Die();

        _timeLived += Time.deltaTime;
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
                player.ActiveTrap(true);
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
    public void Die()
    {
        if (_isDie) return;

        _isDie = true;

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

    #region SetGameManager
    public void SetGameManager(GameManager gm) => _gameManager = gm;
    #endregion
}
