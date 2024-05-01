using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Tooltip("Velocidad de seguimiento (en unidades por segundo).")]
    public float followSpeed = 5.0f;

    [Tooltip("Distancia a la que este objeto dejar� de seguir al jugador.")]
    public float stopDistance = 2.0f;

    [Tooltip("Velocidad de rotaci�n (en grados por segundo).")]
    public float rotationSpeed = 180.0f;

    [Tooltip("Velocidad de movimiento vertical (en unidades por segundo).")]
    public float verticalSpeed = 2.0f;

    private Rigidbody rb;
    private Vector3 initialPosition; // Posici�n inicial del objeto

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactiva la gravedad

        // Guarda la posici�n inicial del objeto para mantener su altura
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Obtiene la posici�n actual del jugador desde el singleton PlayerPosition
        Vector3 playerPosition = PlayerPosition.Instance.GetPlayerPosition();

        // Calcula la direcci�n hacia el jugador solo en el plano horizontal
        Vector3 direction = (new Vector3(playerPosition.x, transform.position.y, playerPosition.z) - transform.position).normalized;

        // Calcula la rotaci�n hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Interpolaci�n de rotaci�n para suavizar el movimiento de rotaci�n
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(playerPosition.x, transform.position.y, playerPosition.z));

        // Si la distancia al jugador es mayor que la distancia de parada, sigue al jugador
        if (distanceToPlayer > stopDistance)
        {
            // Calcula la velocidad basada en la direcci�n y la velocidad de seguimiento
            Vector3 horizontalVelocity = direction * followSpeed;

            // Calcula la velocidad vertical para mantener la altura
            float verticalVelocity = (initialPosition.y - transform.position.y) * verticalSpeed;

            // Combina las velocidades horizontal y vertical
            Vector3 velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

            // Aplica la velocidad al Rigidbody
            rb.velocity = velocity;
        }
        else
        {
            // Si la distancia al jugador es menor o igual que la distancia de parada, det�n el movimiento
            rb.velocity = Vector3.zero;
        }
    }
}