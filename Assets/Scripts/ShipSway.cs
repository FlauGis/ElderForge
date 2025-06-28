using UnityEngine;

public class ShipSway : MonoBehaviour
{
    public float swayAmount = 0.5f; // Розмір гойдання
    public float swaySpeed = 2f; // Швидкість гойдання
    private Vector3 startPosition;

    void Start()
    {
        // Запам'ятовуємо початкову позицію корабля
        startPosition = transform.position;
    }

    void Update()
    {
        // Гойдання по осі Y (можна змінити на інші осі для ефекту на воді)
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.position = startPosition + new Vector3(0, sway, 0);
    }
}
