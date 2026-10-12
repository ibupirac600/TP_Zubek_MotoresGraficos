using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float fuerzaMovimiento = 20f;
    public float frenado = 5f;
    public Transform camara;
    public Transform modelo;
    public float velocidadGiro = 10f;
    public float correccionGiroModelo = 180f;
    public bool invertirControles = false;

    [Header("Salto")]
    public float fuerzaSalto = 6f;
    public bool isGrounded;

    private Rigidbody rb;
    private float inputX;
    private float inputZ;
    private bool quiereSaltar;
    private Vector3 posicionInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionInicial = transform.position;

        if (camara == null && Camera.main != null)
        {
            camara = Camera.main.transform;
        }

        if (modelo == null && transform.childCount > 0)
        {
            modelo = transform.GetChild(0);
        }
    }

 
    void Update()
    {
        inputX = Input.GetAxis("Horizontal"); // A/D o flechas
        inputZ = Input.GetAxis("Vertical");   // W/S o flechas

        if (invertirControles)
        {
            inputX = -inputX;
            inputZ = -inputZ;
        }

        // Solo puede saltar si esta en el piso
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            quiereSaltar = true;
        }

        // Si se cae del nivel, vuelve al inicio
        if (transform.position.y < -10f)
        {
            Reaparecer();
        }
    }

    
    void FixedUpdate()
    {
        // Movimiento relativo a la camara: W siempre aleja al personaje de la camara
        Vector3 adelante = Vector3.forward;
        Vector3 derecha = Vector3.right;
        if (camara != null)
        {
            adelante = camara.forward;
            adelante.y = 0f;
            adelante.Normalize();
            derecha = camara.right;
            derecha.y = 0f;
            derecha.Normalize();
        }

        Vector3 direccion = adelante * inputZ + derecha * inputX;
        if (direccion.magnitude > 1f)
        {
            direccion.Normalize();
        }
        rb.AddForce(direccion * fuerzaMovimiento);

        // Frenado: una fuerza contraria al deslizamiento horizontal (no afecta la caida)
#if UNITY_6000_0_OR_NEWER
        Vector3 velocidad = rb.linearVelocity;
#else
        Vector3 velocidad = rb.velocity;
#endif
        Vector3 velocidadHorizontal = new Vector3(velocidad.x, 0f, velocidad.z);
        rb.AddForce(-velocidadHorizontal * frenado);

        // El modelo gira hacia donde camina
        if (modelo != null && direccion.sqrMagnitude > 0.01f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion) * Quaternion.Euler(0f, correccionGiroModelo, 0f);
            modelo.rotation = Quaternion.Slerp(modelo.rotation, rotacionDeseada, velocidadGiro * Time.fixedDeltaTime);
        }

        if (quiereSaltar)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            quiereSaltar = false;
            isGrounded = false;
        }
    }

    // Deteccion de suelo con la Tag Ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Por si pasa de una pieza Ground a otra 
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Recoleccion de items 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
            if (GameManager.instance != null)
            {
                GameManager.instance.SumarItem();
            }
        }
    }

    void Reaparecer()
    {
        transform.position = posicionInicial;
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = Vector3.zero;
#else
        rb.velocity = Vector3.zero;
#endif
        rb.angularVelocity = Vector3.zero;
    }
}