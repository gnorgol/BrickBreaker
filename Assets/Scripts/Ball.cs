using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;
    public bool inPlay = false;
    public Transform paddle;
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        if (inPlay)
        {
            gameManager.RegisterBall();
        }
    }

    void Update()
    {
        if (!inPlay)
        {
            // Verrouiller la balle sur la raquette avant le lancement
            transform.position = paddle.position + new Vector3(0, 0.5f, 0);

            if (Input.GetButtonDown("Jump"))
            {
                inPlay = true;
                rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ajuster légèrement l'angle de rebond pour éviter les rebonds trop verticaux ou horizontaux
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bottom"))
        {
            gameManager.UnregisterBall();
        }
    }
    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        rb.velocity = rb.velocity.normalized * speed;
    }

    public void DecreaseSpeed(float amount)
    {
        speed = Mathf.Max(speed - amount, 1f); // Pour éviter que la vitesse ne soit trop basse
        rb.velocity = rb.velocity.normalized * speed;
    }

    public void ResetBall()
    {
        inPlay = false;
        rb.velocity = Vector2.zero;

    }
}
