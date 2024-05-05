using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActualizarContadorDrones : MonoBehaviour
{
    void Start()
    {
        // Obtenemos la referencia al componente TextMeshProUGUI del mismo objeto
        TextMeshProUGUI contadorTexto = GetComponent<TextMeshProUGUI>();

        // Comprobamos si el componente TextMeshProUGUI existe en este objeto
        if (contadorTexto == null)
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en el objeto.");
            return;
        }

        // Actualizamos el contador de drones inicialmente
        ActualizarContador();
    }

    void Update()
    {
        // Actualizamos el contador de drones en cada frame (puedes quitar esto si no necesitas actualizaciones constantes)
        ActualizarContador();
    }

    void ActualizarContador()
    {
        // Obtenemos el contador de drones del Singleton
        int contadorDrones = PlayerPosition.Instance.getContadorDrones();

        // Obtenemos la referencia al componente TextMeshProUGUI del mismo objeto
        TextMeshProUGUI contadorTexto = GetComponent<TextMeshProUGUI>();

        // Actualizamos el TextMeshProUGUI con el contador de drones
        if (contadorTexto != null)
        {
            contadorTexto.text = contadorDrones.ToString();
        }
        else
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en el objeto.");
        }
    }
}