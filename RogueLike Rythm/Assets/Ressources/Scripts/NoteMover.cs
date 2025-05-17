using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private float moveSpeed;
    private bool isMoving = false;

    public void InitMovement(float speed)
    {
        moveSpeed = speed;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

    }
}
