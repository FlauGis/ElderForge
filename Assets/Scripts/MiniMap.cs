using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, 0);
    public float smoothSpeed = 5f;
    public bool followRotation = false;

    public Vector2 minBounds; // Мінімальні координати (X, Z)
    public Vector2 maxBounds; // Максимальні координати (X, Z)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Обмежуємо позицію камери в межах заданих координат
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        if (followRotation)
        {
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
