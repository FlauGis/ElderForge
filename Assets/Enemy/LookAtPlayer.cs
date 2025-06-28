using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform camara;

    void LateUpdate()
    {
        transform.LookAt(camara);
    }
}
