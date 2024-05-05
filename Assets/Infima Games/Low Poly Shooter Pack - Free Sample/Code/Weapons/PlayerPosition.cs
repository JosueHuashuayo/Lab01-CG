using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    // Singleton para acceder a la posici�n del jugador desde cualquier parte del c�digo
    int contador_de_drones = 0;

    public void aumentarContadorDrones()
    {
        if(contador_de_drones<15)
            contador_de_drones++;
    }

    public int getContadorDrones()
    {
        return contador_de_drones;
    }

    public static PlayerPosition Instance { get; private set; }

    [Tooltip("Referencia al objeto del jugador.")]
    [SerializeField] private GameObject playerObject;

    // M�todo para obtener la posici�n actual del jugador
    public Vector3 GetPlayerPosition()
    {
        if (playerObject != null)
            return playerObject.transform.position;
        else
        {
            Debug.LogWarning("Player object reference is null! Returning zero vector.");
            return Vector3.zero;
        }
    }

    // M�todo para establecer manualmente la referencia al objeto del jugador
    public void SetPlayerObject(GameObject player)
    {
        playerObject = player;
    }

    void Awake()
    {
        // Configura el singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}