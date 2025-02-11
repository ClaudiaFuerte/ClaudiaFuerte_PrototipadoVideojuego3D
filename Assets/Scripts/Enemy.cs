using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // === CONFIGURACI�N DEL MOVIMIENTO DEL ENEMIGO ===

    [Header("MOVEMENT PLAYER")] // Agrupa las variables en el Inspector
    [SerializeField]
    private int speed = 12; // Velocidad de movimiento del enemigo

    [SerializeField]
    private float distanceToPlayer = 6; // Distancia m�nima antes de que el enemigo deje de avanzar

    GameObject player; // Referencia al jugador

    // === CONFIGURACI�N DEL ATAQUE DEL ENEMIGO ===

    [Header("ENEMY ATTACK")]
    [SerializeField]
    private GameObject bulletPrefab; // Prefab del proyectil del enemigo

    [SerializeField]
    private Transform[] posRotBullet; // Posiciones y rotaciones desde donde se disparan los proyectiles

    [SerializeField]
    private float timeBetweenBullets; // Tiempo entre cada disparo

    [SerializeField]
    private AudioSource shootAudio; // Sonido del disparo

    // === CONFIGURACI�N DE LA VIDA DEL ENEMIGO ===

    [Header("HEALTH BAR")]
    private float maxHealth = 100; // Salud m�xima del enemigo

    private float currentHealth = 100; // Salud actual del enemigo

    private float damageBullet = 50; // Da�o recibido por disparos del jugador

    [SerializeField]
    private ParticleSystem bigExplosion; // Efecto de explosi�n cuando recibe da�o o muere

    void Awake()
    {
        // Se obtiene la referencia del jugador buscando el tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        // Se obtiene la referencia al componente de audio para reproducir el sonido del disparo
        shootAudio = GetComponent<AudioSource>();

        // Se ejecuta la funci�n "Attack" repetidamente cada "timeBetweenBullets" segundos
        InvokeRepeating("Attack", 1, timeBetweenBullets);

        // Se asegura de que la explosi�n est� desactivada al inicio
        bigExplosion.Stop();

        // Inicializa la vida del enemigo con la salud m�xima
        currentHealth = maxHealth;
    }

    // Funci�n que gestiona el ataque del enemigo
    private void Attack()
    {
        // Reproduce el sonido del disparo
        shootAudio.Play();

        // Instancia una bala en cada posici�n de disparo configurada en "posRotBullet"
        for (int i = 0; i < posRotBullet.Length; i++)
        {
            Instantiate(bulletPrefab, posRotBullet[i].position, posRotBullet[i].rotation);
        }
    }

    void Update()
    {
        // Si el jugador no existe, detiene la ejecuci�n del c�digo
        if (player == null)
        {
            return;
        }

        // Hace que el enemigo mire hacia el jugador
        transform.LookAt(player.transform.position);

        // Llama a la funci�n que maneja el movimiento del enemigo
        FollowPlayer();
    }

    // Funci�n que permite al enemigo seguir al jugador
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

    // Funci�n que detecta colisiones con otros objetos
    private void OnTriggerEnter(Collider other)
    {
        // Si el enemigo recibe un impacto de una bala del jugador
        if (other.CompareTag("Bullet"))
        {
            // Reduce la vida del enemigo seg�n el da�o de la bala
            currentHealth -= damageBullet;

            // Activa el efecto de explosi�n cuando recibe da�o
            bigExplosion.Play();

            // Destruye la bala que impact� al enemigo
            Destroy(other.gameObject);

            // Si la vida del enemigo llega a 0 o menos, se ejecuta la funci�n Death()
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    // Funci�n que maneja la muerte del enemigo
    private void Death()
    {
        // Activa el efecto de explosi�n al morir
        bigExplosion.Play();

        // Destruye al enemigo con un retraso de 0.5 segundos para que la explosi�n sea visible
        Destroy(gameObject, 0.5f);
    }
}
