using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodisparo : MonoBehaviour
{
    [SerializeField] private Weapon weapon; // Referencia al script del arma
    [SerializeField] private float interval = 1.0f; // Intervalo entre disparos automáticos

    private float timer = 0.0f; // Temporizador para controlar el intervalo

    private void Update()
    {
        // Si el arma es automática y hay munición, y el temporizador ha alcanzado el intervalo
        if (weapon.IsAutomatic() && weapon.HasAmmunition() && timer >= interval)
        {
            // Dispara
            weapon.Fire();
            // Reinicia el temporizador
            timer = 0.0f;
        }

        // Incrementa el temporizador
        timer += Time.deltaTime;
    }
}
