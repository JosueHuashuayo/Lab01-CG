using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollison : MonoBehaviour
{
    [Tooltip("Tag del objeto que causará la destrucción.")]
    [SerializeField] private string collisionTag = "Balazo"; // Cambia esto al tag que necesites

    [Tooltip("Objeto de explosión a instanciar.")]
    [SerializeField] private GameObject explosionPrefab;

    [Tooltip("Cantidad de disparos necesarios para destruir este objeto.")]
    [SerializeField] private int life = 3; // Cambia esto a la cantidad de disparos que deseas

    [SerializeField] public FollowPlayer follow;

    private int currentLife; // Vida actual del objeto

    private void Start()
    {
        currentLife = life; // Inicializa la vida actual con la cantidad de vida definida
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto colisionador tiene el tag correcto
        if (collision.gameObject.CompareTag(collisionTag))
        {
            // Reduce la vida del objeto
            currentLife--;

            // Si la vida llega a cero, destruye el objeto
            if (currentLife <= 0 && follow.isActive)
            {
                // Instancia el objeto de explosión si está definido
                if (explosionPrefab != null)
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                // Destruye este objeto
                PlayerPosition.Instance.aumentarContadorDrones();
                //Destroy(gameObject);

                follow.isActive = false;
            }
        }
    }
}