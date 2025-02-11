using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // === CONFIGURACIÓN DEL MOVIMIENTO ===

    private int speed = 50; // Velocidad de movimiento del jugador
    private int turnSpeed = 400; // Velocidad de giro del jugador

    // === CONFIGURACIÓN DEL ATAQUE ===

    [SerializeField]
    private GameObject bulletPrefab; // Prefab de la bala que dispara el jugador

    [SerializeField]
    private Transform[] posRotBullet; // Posiciones desde donde se disparan las balas

    [SerializeField]
    private AudioSource shootAudio; // Sonido del disparo del jugador

    // === CONFIGURACIÓN DE LA VIDA DEL JUGADOR ===

    [Header("Player Health")]
    private float maxHealth = 100; // Salud máxima del jugador
    private float currentHealth = 100; // Salud actual del jugador
    private float damageBullet = 5; // Daño recibido por las balas enemigas

    [SerializeField]
    private Image lifeBar; // Barra de vida del jugador en la UI

    // === EFECTOS VISUALES ===

    [Header("FX Damage")]
    [SerializeField]
    private ParticleSystem explosion; // Efecto de explosión cuando el jugador recibe daño

    private Rigidbody rb; // Referencia al Rigidbody del jugador

    [SerializeField]
    private GameManager gameManager; // Referencia al GameManager para manejar el Game Over

    private void Awake()
    {
        // Se obtiene el Rigidbody del jugador y se congela su rotación para evitar giros no deseados
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Se obtiene la referencia al componente de audio del disparo
        shootAudio = GetComponent<AudioSource>();

        // Inicializa la vida del jugador
        currentHealth = maxHealth;
        lifeBar.fillAmount = 1;

        // Bloquea el cursor en el centro de la pantalla para mejorar el control
        Cursor.lockState = CursorLockMode.Locked;

        // Asegura que la explosión esté desactivada al inicio
        explosion.Stop();
    }

    private void Update()
    {
        // Maneja el ataque, movimiento y giro del jugador en cada frame
        Attack();
        Movement();
        Turning();
    }

    // === ATAQUE DEL JUGADOR ===
    private void Attack()
    {
        // Si el jugador presiona el botón izquierdo del ratón (disparo)
        if (Input.GetMouseButtonDown(0))
        {
            // Instancia una bala en cada posición configurada en "posRotBullet"
            for (int i = 0; i < posRotBullet.Length; i++)
            {
                Instantiate(bulletPrefab, posRotBullet[i].position, posRotBullet[i].rotation);
            }

            // Reproduce el sonido del disparo
            shootAudio.Play();
        }
    }

    // === MOVIMIENTO DEL JUGADOR ===
    private void Movement()
    {
        // Obtiene la entrada del teclado (WASD o flechas)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula la dirección del movimiento en función de la orientación del jugador
        Vector3 direction = transform.forward * vertical + transform.right * horizontal;

        // Aplica la velocidad en la dirección calculada, manteniendo la velocidad vertical actual
        rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
    }

    // === ROTACIÓN DEL JUGADOR ===
    private void Turning()
    {
        // Captura el movimiento del ratón en los ejes X (horizontal) y Y (vertical)
        float xMouse = Input.GetAxis("Mouse X");
        float yMouse = Input.GetAxis("Mouse Y");

        // Aplica la rotación en el eje Y para girar al jugador (ignora la rotación en X)
        Vector3 rotation = new Vector3(0, xMouse, 0) * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    // === DETECCIÓN DE COLISIONES ===
    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador es impactado por una bala enemiga
        if (other.CompareTag("BulletEnemy"))
        {
            // Reduce la vida del jugador
            currentHealth -= damageBullet;

            // Actualiza la barra de vida en la interfaz
            lifeBar.fillAmount = currentHealth / maxHealth;

            // Destruye la bala enemiga después del impacto
            Destroy(other.gameObject);

            // Reproduce el efecto de explosión
            explosion.Play();
        }

        // Si la vida del jugador llega a 0, ejecuta la función Death() y activa el Game Over
        if (currentHealth <= 0)
        {
            Death();
            gameManager.GameOver();
        }
    }

    // === MANEJO DE LA MUERTE DEL JUGADOR ===
    private void Death()
    {
        // Desvincula la cámara del jugador para evitar errores visuales al destruirlo
        Camera.main.transform.SetParent(null);

        // Destruye el objeto del jugador
        Destroy(gameObject);
    }
}

