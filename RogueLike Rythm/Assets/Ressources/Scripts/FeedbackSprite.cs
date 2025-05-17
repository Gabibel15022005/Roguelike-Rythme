using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FeedbackSprite : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float scaleUpFactor = 1.2f;
    [SerializeField] private float fadeDuration = 0.8f;

    private float timer = 0f;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Scale animation
        float scaleT = Mathf.Min(timer / (lifetime * 0.2f), 1f);
        transform.localScale = Vector3.Lerp(Vector3.zero, initialScale * scaleUpFactor, scaleT);

        // Float upwards
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out
        float fadeT = Mathf.Clamp01((lifetime - timer) / fadeDuration);
        Color color = spriteRenderer.color;
        color.a = fadeT;
        spriteRenderer.color = color;

        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}