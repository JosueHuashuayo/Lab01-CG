using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controler : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad de movimiento del personaje
    public Animator animator; // Referencia al componente Animator


    private bool waAxisPressed = false;
    Rigidbody rb; // Referencia al componente Rigidbody

    void Start()
    {
        // Obtener la referencia al componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verificar si se presionó la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space)) Prepararse(); // Llamar a la función Prepararse al presionar espacio
        if (Input.GetKeyUp(KeyCode.Space)) Quieto(); // Llamar a la función Prepararse al presionar espacio

        if (animator.GetBool("Prepararse")) return;

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        bool axisPressed = movimientoHorizontal != 0f || movimientoVertical != 0f;
        if (axisPressed)
        {
            Correr(); // Llamar a la función Correr si hay movimiento
            Mover(movimientoHorizontal, movimientoVertical);
        }
        else
        {
            if (waAxisPressed)
            {
                Quieto();
            }
        }
        waAxisPressed = axisPressed;

    }

    void Mover(float horizontal, float vertical)
    {
        // Calcular la dirección del movimiento
        Vector3 movimiento = new Vector3(horizontal, 0f, vertical).normalized * velocidad * Time.deltaTime;

        rb.MovePosition(transform.position + movimiento);
        
        Quaternion rotacionDeseada = Quaternion.LookRotation(movimiento);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionDeseada, 1000f*Time.deltaTime);

    }

    void Correr()
    {
        animator.SetBool("Correr", true);
        animator.SetBool("Prepararse", false);
        animator.SetBool("Lanzar", false);
        animator.SetBool("Quieto", false);
    }

    void Quieto()
    {
        animator.SetBool("Correr", false);
        animator.SetBool("Prepararse", false);
        animator.SetBool("Lanzar", false);
        animator.SetBool("Quieto", true);
    }

    void Prepararse()
    {
        animator.SetBool("Correr", false);
        animator.SetBool("Prepararse", true);
        animator.SetBool("Lanzar", false);
        animator.SetBool("Quieto", false);
    }

    void Lanzar()
    {
        animator.SetBool("Correr", false);
        animator.SetBool("Prepararse", false);
        animator.SetBool("Lanzar", true);
        animator.SetBool("Quieto", false);
    }
}