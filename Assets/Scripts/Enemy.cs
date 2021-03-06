using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Range(0f, 0.01f)] float speedModifier;

    Folower2D folower;

    private void Awake()
    {
        folower = GetComponent<Folower2D>();
    }

    void FixedUpdate()
    {
        folower.speed += speedModifier;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Enemy:

                break;
        }
    }

    public IEnumerator ChangeScale(float changeSpeed)
    {
        while (transform.localScale.magnitude > 0.1)
        {
            yield return null;
        }
    }
}
