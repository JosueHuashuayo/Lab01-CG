using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodisparo : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float interval = 1.0f;
    [SerializeField] public FollowPlayer follow;

    public AudioClip[] firingSounds; // Lista de sonidos de disparo

    [Range(0, 1)]
    public float maxVolume = 1f; // Volumen m�ximo del sonido

    [Range(1, 10)]
    public float maxDistance = 7f; // Distancia m�xima en la que el sonido se escucha a volumen m�ximo

    private float timer = 0.0f; // Temporizador para controlar el intervalo

    private void Update()
    {
        if (follow.isActive)
        {
            // Verificar si hay una l�nea de visi�n clara al jugador
            if (weapon.IsAutomatic() && weapon.HasAmmunition() && timer >= interval && HasLineOfSightToPlayer())
            {
                // Disparar
                weapon.Fire();
                // Reproducir sonido de disparo
                PlayRandomFiringSound();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        // Calcular la direcci�n hacia el jugador
        Vector3 direction = PlayerPosition.Instance.GetPlayerPosition() - transform.position;

        // Lanzar un rayo hacia el jugador
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity))
        {
            // Verificar si el rayo golpea al jugador
            if (hit.collider.CompareTag("Player"))
            {
                return true; // Hay visi�n directa al jugador
            }
        }
        // Si el rayo no golpea al jugador, no hay visi�n directa
        return false;
    }

    private void PlayRandomFiringSound()
    {
        // Si la lista de sonidos de disparo est� vac�a, salimos
        if (firingSounds == null || firingSounds.Length == 0)
            return;

        // Selecciona aleatoriamente un sonido de la lista
        AudioClip selectedClip = firingSounds[Random.Range(0, firingSounds.Length)];

        // Calcula la distancia entre el objeto y el jugador
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerPosition.Instance.GetPlayerPosition());

        // Calcula el volumen basado en la distancia usando una funci�n logar�tmica
        float volume = Mathf.Clamp01(1f - Mathf.Log10(distanceToPlayer) / Mathf.Log10(maxDistance));

        // Multiplica por el volumen m�ximo para obtener el volumen final
        volume *= maxVolume;

        // Reproduce el sonido en la posici�n del objeto con el volumen calculado
        AudioSource.PlayClipAtPoint(selectedClip, transform.position, volume);
    }
}
