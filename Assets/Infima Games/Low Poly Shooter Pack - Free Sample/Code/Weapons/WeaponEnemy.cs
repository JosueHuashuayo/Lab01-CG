using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class WeaponEnemy : Weapon
    {
        //protected override void Awake()
        //{
        //}

        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //Se nesecita un punto de disparo
            if (muzzleBehaviour == null)
                return;

            //Se nesecita la camara para determinar la dirección
            if (shootDirection == null)
                return;

            //Get Muzzle Socket. This is the point we fire from.
            Transform muzzleSocket = muzzleBehaviour.GetSocket();

            //Play the firing animation.
            //const string stateName = "Fire";
            //animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //Play all muzzle effects.
            muzzleBehaviour.Effect();


            //Determinar la rotación en la que queremos disparar nuestro proyectil. La dirección de la camara
            Quaternion rotation = Quaternion.LookRotation(muzzleSocket.forward * 1000.0f - muzzleSocket.position);

            //Spawn projectile from the projectile spawn point.
            GameObject projectile = Instantiate(prefabProjectile, muzzleSocket.position, rotation);
            //Add velocity to the projectile.
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
        }
    }
}