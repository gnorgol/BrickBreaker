using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{
    public float speed = 10f;
    private float screenWidthUnits;
    private bool expandBonusActive = false;
    private Vector3 originalScale;

    void Start()
    {
        screenWidthUnits = Camera.main.orthographicSize * Camera.main.aspect;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        // Limiter la raquette aux bords de l'écran
        position.x = Mathf.Clamp(position.x, -screenWidthUnits + 1, screenWidthUnits - 1);
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
