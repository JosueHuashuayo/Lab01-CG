using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public bool isActive = true;

    [Tooltip("Velocidad de seguimiento (en unidades por segundo).")]
    public float followSpeed = 5.0f;

    [Tooltip("Distancia a la que este objeto dejará de seguir al jugador.")]
    public float stopDistance = 2.0f;

    [Tooltip("Velocidad de rotación (en grados por segundo).")]
    public float rotationSpeed = 180.0f;

    [Tooltip("Velocidad de movimiento vertical (en unidades por segundo).")]
    public float verticalSpeed = 2.0f;

    // Ajustes para el movimiento vertical oscilatorio
    public float baseAmplitude = 0.5f; // Amplitud base de la oscilación
    public float baseFrequency = 1.0f; // Frecuencia base de la oscilación (en Hz)
    public float amplitudeVariation = 0.1f; // Variación de amplitud
    public float frequencyVariation = 0.1f; // Variación de frecuencia

    public float heightCorrectionForce = 2.0f; // Fuerza de corrección de altura

    private Rigidbody rb;
    private Vector3 initialPosition; // Posición inicial del objeto
    private float randomAmplitude;
    private float randomFrequency;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactiva la gravedad

        // Guarda la posición inicial del objeto para mantener su altura
        initialPosition = transform.position;

        // Calcula valores aleatorios de amplitud y frecuencia
        randomAmplitude = Random.Range(-amplitudeVariation, amplitudeVariation);
        randomFrequency = Random.Range(-frequencyVariation, frequencyVariation);
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            // Obtiene la posición actual del jugador desde el singleton PlayerPosition
            Vector3 playerPosition = PlayerPosition.Instance.GetPlayerPosition();

            // Calcula la dirección hacia el jugador solo en el plano horizontal
            Vector3 direction = (new Vector3(playerPosition.x, transform.position.y, playerPosition.z) - transform.position).normalized;

            // Calcula la rotación hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Interpolación de rotación para suavizar el movimiento de rotación
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            // Calcula la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(playerPosition.x, transform.position.y, playerPosition.z));

            // Si la distancia al jugador es mayor que la distancia de parada, sigue al jugador
            Vector3 horizontalVelocity = new Vector3(0, 0, 0);


            if (distanceToPlayer > stopDistance && Vector3.Distance(playerPosition, transform.position) < 20)
            {
                // Calcula la velocidad basada en la dirección y la velocidad de seguimiento
                horizontalVelocity = direction * followSpeed;
            }

            // Calcula la amplitud y frecuencia con aleatoriedad
            float amplitude = baseAmplitude + randomAmplitude;
            float frequency = baseFrequency + randomFrequency;

            // Calcula la velocidad vertical para mantener la altura
            float targetHeight = initialPosition.y;
            float currentHeight = transform.position.y;
            float heightDifference = targetHeight - currentHeight;
            float heightCorrectionVelocity = heightDifference * 100;

            float verticalVelocity = Mathf.Sin(Time.time * 2 * Mathf.PI * frequency) * amplitude * verticalSpeed;

            // Combina las velocidades horizontal y vertical
            Vector3 velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

            // Aplica la velocidad al Rigidbody
            rb.velocity = velocity;

            // Aplica la fuerza de corrección de altura
            rb.AddForce(Vector3.up * heightCorrectionVelocity, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * 10);
            rb.mass = 1;
        }
    }
}