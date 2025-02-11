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
        // Destruye la bala autom�ticamente despu�s de 5 segundos para evitar acumulaci�n de objetos en la escena
        Destroy(gameObject, 5);
    }
}

