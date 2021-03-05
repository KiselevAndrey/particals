using UnityEngine;

enum MoveType { Lerp, MoveTowards}

public class Folower2D : MonoBehaviour
{
    [SerializeField] Transform target;
    public float speed;
    [SerializeField] MoveType moveType;

    public float startZ;

    private void Awake()
    {
        startZ = transform.position.z;
    }

    private void Start()
    {
        if (target == null) target = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 temp = target.transform.position;
        temp.z = startZ;

        switch (moveType)
        {
            case MoveType.Lerp:
                transform.position = Vector3.Lerp(transform.position, temp, speed * Time.deltaTime);
                break;
            case MoveType.MoveTowards:
                transform.position = Vector3.MoveTowards(transform.position, temp, speed * Time.deltaTime);
                break;
        }

    }
}
