using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float speedModifier;

    Folower2D folower;

    private void Awake()
    {
        folower = GetComponent<Folower2D>();
    }

    void FixedUpdate()
    {
        folower.speed += speedModifier;
    }
}
