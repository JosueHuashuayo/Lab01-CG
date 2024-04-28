using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del personaje
    public Animator animator; // Referencia al componente Animator

    Rigidbody rb; // Referencia al componente Rigidbody

    void Start()
    {
        // Obtener la referencia al componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verificar si se presionó la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Prepararse(); // Llamar a la función Prepararse al presionar espacio
        }

        // Si está preparándose, salir de la función de actualización
        if (animator.GetBool("Prepararse"))
            return;

        // Obtener la entrada del teclado
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Controlar las animaciones según el movimiento
        if (movimientoHorizontal != 0f || movimientoVertical != 0f)
        {
            Correr(); // Llamar a la función Correr si hay movimiento
            Mover(movimientoHorizontal, movimientoVertical);
        }
        else
        {
            Quieto(); // Llamar a la función Quieto si no hay movimiento
        }
    }

    void Mover(float horizontal, float vertical)
    {
        // Calcular la dirección del movimiento
        Vector3 movimiento = new Vector3(horizontal, 0f, vertical) * velocidad * Time.deltaTime;

        // Aplicar el movimiento al Rigidbody
        rb.MovePosition(transform.position + movimiento);

        // Rotar el personaje hacia la dirección del movimiento
        if (movimiento != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movimiento);
        }
    }

    void Correr()
    {
        animator.SetBool("Correr", true);
        animator.SetBool("Quieto", false);
    }

    void Quieto()
    {
        animator.SetBool("Correr", false);
        animator.SetBool("Quieto", true);
    }

    void Prepararse()
    {
        // Cambiar el valor del parámetro Prepararse a true
        animator.SetBool("Prepararse", !animator.GetBool("Prepararse"));
    }
}