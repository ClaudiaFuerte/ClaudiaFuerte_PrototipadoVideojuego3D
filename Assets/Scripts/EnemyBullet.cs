using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Velocidad de la bala enemiga
    private float speed = 100f;

    void Update()
    {
        // Mueve la bala hacia adelante en cada frame, usando su velocidad y el tiempo transcurrido
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Start()
    {
        // Destruye la bala automáticamente después de 5 segundos para evitar acumulación de objetos en la escena
        Destroy(gameObject, 5);
    }
}

