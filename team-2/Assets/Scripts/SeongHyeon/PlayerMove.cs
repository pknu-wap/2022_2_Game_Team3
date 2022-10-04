using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public UnityEvent onPlayerDead;
    public int speed;
    public int jumpSpeed;

    private bool isGround = false;
    // Start is called before the first frame update
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        Vector3 movingWay = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        transform.Translate(movingWay * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Flame"))
        {
            onPlayerDead.Invoke();
            Debug.Log("Dead");
            SceneManager.LoadScene("Room2");
        }
    }
}
