using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador;
    public Vector3 distancia = new Vector3(0f, 5f, -8f);
    public float suavizado = 5f;

    [Header("Control con el mouse")]
    public bool usarClicDerecho = true;
    public float sensibilidadMouse = 3f;
    public float velocidadZoom = 2f;
    public float distanciaMinima = 2f;
    public float distanciaMaxima = 15f;
    public float anguloMinimo = -10f;
    public float anguloMaximo = 70f;
    public float alturaMirada = 0.5f;

    private float giroHorizontal;
    private float giroVertical;
    private float distanciaActual;
    private Vector3 puntoSeguido;

    void Start()
    {
        if (jugador == null)
        {
            GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
            if (objetoJugador != null)
            {
                jugador = objetoJugador.transform;
            }
        }

        if (jugador == null)
        {
            Debug.LogError("CameraFollow: no se encontro ningun objeto con la Tag Player.");
            enabled = false;
            return;
        }

        // Convierte la distancia inicial en angulos para poder girar alrededor del jugador
        distanciaActual = Mathf.Clamp(distancia.magnitude, distanciaMinima, distanciaMaxima);
        giroHorizontal = Mathf.Atan2(-distancia.x, -distancia.z) * Mathf.Rad2Deg;
        giroVertical = Mathf.Asin(Mathf.Clamp(distancia.y / distancia.magnitude, -1f, 1f)) * Mathf.Rad2Deg;

        puntoSeguido = jugador.position;
        ActualizarPosicion();
    }

    void LateUpdate()
    {
        // Girar con el mouse (manteniendo el clic derecho, o siempre si usarClicDerecho esta apagado)
        if (!usarClicDerecho || Input.GetMouseButton(1))
        {
            giroHorizontal += Input.GetAxis("Mouse X") * sensibilidadMouse;
            giroVertical -= Input.GetAxis("Mouse Y") * sensibilidadMouse;
            giroVertical = Mathf.Clamp(giroVertical, anguloMinimo, anguloMaximo);
        }

        // Acercar y alejar con la ruedita
        distanciaActual -= Input.mouseScrollDelta.y * velocidadZoom;
        distanciaActual = Mathf.Clamp(distanciaActual, distanciaMinima, distanciaMaxima);

        puntoSeguido = Vector3.Lerp(puntoSeguido, jugador.position, suavizado * Time.deltaTime);
        ActualizarPosicion();
    }

    void ActualizarPosicion()
    {
        Quaternion rotacion = Quaternion.Euler(giroVertical, giroHorizontal, 0f);
        Vector3 desplazamiento = rotacion * new Vector3(0f, 0f, -distanciaActual);
        transform.position = puntoSeguido + desplazamiento;
        transform.LookAt(puntoSeguido + Vector3.up * alturaMirada);
    }
}
