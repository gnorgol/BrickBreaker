using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum BonusType { SpeedUp, SlowDown, ExpandPaddle, MultiBall }
    public BonusType bonusType;
    public float fallSpeed = 3f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paddle"))
        {
            Paddle paddle = collision.GetComponent<Paddle>();
            Ball ball = FindObjectOfType<Ball>();
            GameManager gameManager = FindObjectOfType<GameManager>();

            switch (bonusType)
            {
                case BonusType.SpeedUp:
                    ball.IncreaseSpeed(2f, 10f);
                    break;

                case BonusType.SlowDown:
                    ball.DecreaseSpeed(2f, 10f);
                    break;

                case BonusType.ExpandPaddle:
                    paddle.Expand(2f, 10f);
                    break;

                case BonusType.MultiBall:
                    gameManager.SpawnExtraBalls(2);
                    break;
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}
