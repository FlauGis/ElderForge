using UnityEngine;

public class PersistentLight : MonoBehaviour
{
    private static PersistentLight instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // якщо об'Їкт уже ≥снуЇ, знищуЇмо дубль
        }
    }
}
