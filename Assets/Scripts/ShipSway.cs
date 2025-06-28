using UnityEngine;

public class ShipSway : MonoBehaviour
{
    public float swayAmount = 0.5f; // ����� ��������
    public float swaySpeed = 2f; // �������� ��������
    private Vector3 startPosition;

    void Start()
    {
        // �����'������� ��������� ������� �������
        startPosition = transform.position;
    }

    void Update()
    {
        // �������� �� �� Y (����� ������ �� ���� �� ��� ������ �� ���)
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.position = startPosition + new Vector3(0, sway, 0);
    }
}
