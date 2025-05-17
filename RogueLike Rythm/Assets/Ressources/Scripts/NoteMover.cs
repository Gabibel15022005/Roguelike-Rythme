using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private Vector3 targetPosition;
    private float travelTime;
    private float elapsedTime;
    private Vector3 startPosition;
    private bool isMoving = false;

    public void MoveTo(Vector3 target, float duration)
    {
        startPosition = transform.position;
        targetPosition = new Vector3(target.x, startPosition.y, startPosition.z);
        travelTime = duration;
        elapsedTime = 0f;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / travelTime);
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1f)
        {
            isMoving = false;
        }
    }
}
