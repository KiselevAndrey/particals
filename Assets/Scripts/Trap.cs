using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField, Min(0)] float lifeTime;
    [SerializeField] Collider2D colider;
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem explosion;

    List<Enemy> cathedEnemes = new List<Enemy>();

    void Update()
    {
        if (lifeTime <= 0) KillEnemies();
        lifeTime -= Time.deltaTime;
    }

    void KillEnemies()
    {
        for (int i = 0; i < cathedEnemes.Count; i++)
        {
            cathedEnemes[i].Die();
        }
        Die();
    }

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Enemy:
                Enemy enemy = collision.GetComponent<Enemy>();
                cathedEnemes.Add(enemy);
                enemy.folower.target = transform;
                enemy.colider.enabled = false;
                break;
        }
    }
    #endregion

    #region Die
    void Die()
    {
        colider.enabled = false;
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion
}
