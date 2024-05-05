using UnityEngine;
using TMPro;

public class ActivarTexto : MonoBehaviour
{
    // Referencia al TextMeshProUGUI que se activará
    public TextMeshProUGUI textoContador;

    // Referencia al Singleton PlayerPosition
 

    // Variable para verificar si el texto ya ha sido activado
    private bool textoActivado = false;

    void Start()
    {
        // Comprobamos si se ha asignado el TextMeshProUGUI
        if (textoContador == null)
        {
            Debug.LogError("El TextMeshProUGUI no está asignado en el script ActivarTextoCuandoContador15.");
            return;
        }

    }

    void Update()
    {
        // Verificamos si el contador alcanzó 15 y si el texto no ha sido activado
        if (PlayerPosition.Instance.getContadorDrones() >= 15 && !textoActivado)
        {
            // Activamos el TextMeshProUGUI
            textoContador.gameObject.SetActive(true);
            textoActivado = true;
        }
    }
}

