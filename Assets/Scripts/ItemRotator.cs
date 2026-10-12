using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public float velocidadGiro = 90f;

    void Update()
    {
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f, Space.World);
    }
}
