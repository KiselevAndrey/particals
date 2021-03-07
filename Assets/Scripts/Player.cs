using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject trapPrefab;
    [SerializeField] ParticleSystem trapAura;

    public bool _canHunting = true;

    void Update()
    {
        Move();
        Hunting();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += Input.GetAxis(AxesNames.Horizontal);
        temp.y += Input.GetAxis(AxesNames.Vertical);

        transform.position = Vector3.MoveTowards(transform.position, temp, speed * Time.deltaTime);
    }


    #region Trap
    void Hunting()
    {
        if (Input.GetMouseButtonDown(0) && _canHunting)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;

            Instantiate(trapPrefab, mouseWorldPos, Quaternion.identity);
            ActiveTrap(false);
        }
    }

    public void ActiveTrap(bool value)
    {
        _canHunting = value;
        if (value) trapAura.Play();
        else trapAura.Stop();
    }
    #endregion

    public void Hit()
    {
        print("hit");
    }
}
