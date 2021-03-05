using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        Vector3 temp = transform.position;
        temp.x += Input.GetAxis(AxesNames.Horizontal);
        temp.y += Input.GetAxis(AxesNames.Vertical);

        transform.position = Vector3.MoveTowards(transform.position, temp, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.tag);
    }
}
