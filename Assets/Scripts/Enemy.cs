using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // === CONFIGURACIÓN DEL MOVIMIENTO DEL ENEMIGO ===

    [Header("MOVEMENT PLAYER")] // Agrupa las variables en el Inspector
    [SerializeField]
    private int speed = 12; // Velocidad de movimiento del enemigo

    [SerializeField]
    private float distanceToPlayer = 6; // Distancia mínima antes de que el enemigo deje de avanzar

    GameObject player; // Referencia al jugador

    // === CONFIGURACIÓN DEL ATAQUE DEL ENEMIGO ===

    [Header("ENEMY ATTACK")]
    [SerializeField]
    private GameObject bulletPrefab; // Prefab del proyectil del enemigo

    [SerializeField]
    private Transform[] posRotBullet; // Posiciones y rotaciones desde donde se disparan los proyectiles

    [SerializeField]
    private float timeBetweenBullets; // Tiempo entre cada disparo

    [SerializeField]
    private AudioSource shootAudio; // Sonido del disparo

    // === CONFIGURACIÓN DE LA VIDA DEL ENEMIGO ===

    [Header("HEALTH BAR")]
    private float maxHealth = 100; // Salud máxima del enemigo

    private float currentHealth = 100; // Salud actual del enemigo

    private float damageBullet = 50; // Daño recibido por disparos del jugador

    [SerializeField]
    private ParticleSystem bigExplosion; // Efecto de explosión cuando recibe daño o muere

    void Awake()
    {
        // Se obtiene la referencia del jugador buscando el tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        // Se obtiene la referencia al componente de audio para reproducir el sonido del disparo
        shootAudio = GetComponent<AudioSource>();

        // Se ejecuta la función "Attack" repetidamente cada "timeBetweenBullets" segundos
        InvokeRepeating("Attack", 1, timeBetweenBullets);

        // Se asegura de que la explosión esté desactivada al inicio
        bigExplosion.Stop();

        // Inicializa la vida del enemigo con la salud máxima
        currentHealth = maxHealth;
    }

    // Función que gestiona el ataque del enemigo
    private void Attack()
    {
        // Reproduce el sonido del disparo
        shootAudio.Play();

        // Instancia una bala en cada posición de disparo configurada en "posRotBullet"
        for (int i = 0; i < posRotBullet.Length; i++)
        {
            Instantiate(bulletPrefab, posRotBullet[i].position, posRotBullet[i].rotation);
        }
    }

    void Update()
    {
        // Si el jugador no existe, detiene la ejecución del código
        if (player == null)
        {
            return;
        }

        // Hace que el enemigo mire hacia el jugador
        transform.LookAt(player.transform.position);

        // Llama a la función que maneja el movimiento del enemigo
        FollowPlayer();
    }

    // Función que permite al enemigo seguir al jugador
    private void FollowPlayer()
    {
        // Calcula la distancia entre el enemigo y el jugador
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Si la distancia es mayor a "distanceToPlayer", el enemigo avanza hacia el jugador
        if (distance > distanceToPlayer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    // Función que detecta colisiones con otros objetos
    private void OnTriggerEnter(Collider other)
    {
        // Si el enemigo recibe un impacto de una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reduce la vida del enemigo según el daño de la bala
            currentHealth -= damageBullet;

            // Activa el efecto de explosión cuando recibe daño
            bigExplosion.Play();

            // Destruye la bala que impactó al enemigo
            Destroy(other.gameObject);

            // Si la vida del enemigo llega a 0 o menos, se ejecuta la función Death()
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    // Función que maneja la muerte del enemigo
    private void Death()
    {
        // Activa el efecto de explosión al morir
        bigExplosion.Play();

        // Destruye al enemigo con un retraso de 0.5 segundos para que la explosión sea visible
        Destroy(gameObject, 0.5f);
    }
}
