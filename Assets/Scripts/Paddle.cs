using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    private float screenWidthUnits;
    private bool expandBonusActive = false;
    private Vector3 originalScale;
    public Camera Camera;
    public InputActionReference moveAction;
    public Collider2D leftWall;
    public Collider2D rightWall;

    void Start()
    {
        screenWidthUnits = Camera.orthographicSize * Camera.aspect;
        originalScale = transform.localScale;
    }
    void OnEnable()
    {
        moveAction.action.Enable();
    }
    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        Vector2 inputVector = moveAction.action.ReadValue<Vector2>();
        float horizontal = inputVector.x;
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;

        // Limiter la raquette aux murs
        float leftLimit = leftWall.bounds.max.x + transform.localScale.x / 2;
        float rightLimit = rightWall.bounds.min.x - transform.localScale.x / 2;
        position.x = Mathf.Clamp(position.x, leftLimit, rightLimit);

        transform.position = position;
    }


    public void Expand(float scaleMultiplier, float duration)
    {
        if (!expandBonusActive)
        {
            expandBonusActive = true;
            transform.localScale = new Vector3(transform.localScale.x * scaleMultiplier, transform.localScale.y, transform.localScale.z);
            StartCoroutine(ResetPaddleAfterDuration(duration));
        }
    }

    private IEnumerator ResetPaddleAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.localScale = originalScale;
        expandBonusActive = false;
    }

    public void ResetPaddle()
    {
        // Réinitialiser la taille et la position de la raquette
        transform.localScale = originalScale;
        transform.position = new Vector3(0f, transform.position.y, 0f);
    }
}
