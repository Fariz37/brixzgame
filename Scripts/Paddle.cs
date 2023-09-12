using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public float speed = 30f;
    public float maxBounceAngle = 75f;

    private Vector3 initialPosition; // Menyimpan posisi awal player

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position; // Menyimpan posisi awal
    }

    private void Start()
    {
        ResetPlayer(); // Memanggil ResetPlayer saat permainan dimulai
    }

    public void ResetPlayer()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = initialPosition; // Mengembalikan player ke posisi awal
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            rigidbody.AddForce(direction * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bola bola = collision.gameObject.GetComponent<Bola>();

        if (bola != null)
        {
            Vector2 playerPosition = transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = playerPosition.x - contactPoint.x;
            float maxOffset = collision.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, bola.rigidbody.velocity);
            float bounceAngle = (offset / maxOffset) * maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBounceAngle, maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            bola.rigidbody.velocity = rotation * Vector2.up * bola.rigidbody.velocity.magnitude;
        }
    }
}
