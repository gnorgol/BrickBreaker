using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;
    public bool inPlay = false;
    public Transform paddle;
    public GameManager gameManager;

    private bool speedBonusActive = false;
    private bool slowBonusActive = false;
    private bool paddleExpandBonusActive = false;
    private float originalSpeed;
    private Vector3 originalPaddleScale;

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

    public void IncreaseSpeed(float amount, float duration)
    {
        if (!speedBonusActive)
        {
            speedBonusActive = true;
            originalSpeed = speed;
            speed += amount;
            rb.velocity = rb.velocity.normalized * speed;
            StartCoroutine(ResetSpeedAfterDuration(duration));
        }
    }

    private IEnumerator ResetSpeedAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        rb.velocity = rb.velocity.normalized * speed;
        speedBonusActive = false;
    }

    public void DecreaseSpeed(float amount, float duration)
    {
        if (!slowBonusActive)
        {
            slowBonusActive = true;
            originalSpeed = speed;
            speed = Mathf.Max(speed - amount, 1f); // Pour éviter que la vitesse ne soit trop basse
            rb.velocity = rb.velocity.normalized * speed;
            StartCoroutine(ResetSlowAfterDuration(duration));
        }
    }

    private IEnumerator ResetSlowAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        rb.velocity = rb.velocity.normalized * speed;
        slowBonusActive = false;
    }



    public void ResetBall()
    {
        inPlay = false;
        rb.velocity = Vector2.zero;
    }
}
