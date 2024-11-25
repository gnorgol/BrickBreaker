using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;
    public bool inPlay = false;
    public Transform paddle;
    public GameManager gameManager;

    private bool speedBonusActive = false;
    private bool slowBonusActive = false;
    private float originalSpeed;
    public InputActionReference ShootAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        if (inPlay)
        {
            gameManager.RegisterBall();
        }
    }

    void OnEnable()
    {
        ShootAction.action.Enable();
        ShootAction.action.performed += ctx => LaunchBall();
    }

    void OnDisable()
    {
        ShootAction.action.Disable();
        ShootAction.action.performed -= ctx => LaunchBall();
    }

    private void Update()
    {
        if (!inPlay)
        {
            // Verrouiller la balle sur la raquette avant le lancement
            transform.position = paddle.position + new Vector3(0, 0.7f, 0);
        }
    }

    private void LaunchBall()
    {
        if (!inPlay)
        {
            inPlay = true;
            rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Calculer l'angle de rebond en fonction de l'endroit où la balle touche la raquette
            float hitFactor = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 direction = new Vector2(hitFactor, 1).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            // Ajuster légèrement l'angle de rebond pour éviter les rebonds trop verticaux ou horizontaux
            Vector2 direction = rb.velocity.normalized;
            if (Mathf.Abs(direction.x) < 0.1f)
            {
                direction.x = direction.x < 0 ? -0.1f : 0.1f;
            }
            if (Mathf.Abs(direction.y) < 0.1f)
            {
                direction.y = direction.y < 0 ? -0.1f : 0.1f;
            }
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
