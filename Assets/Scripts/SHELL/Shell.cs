using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    //The time in the air before the shell is removed
    public float m_MaxLifeTime = 2f;
    //The amount of damage done if the explosion is centered on a tank
    public float m_MaxDamage = 34f;
    //The maximum distance away from the explosion tanks can be and are still affected
    public float m_ExplosionRadius = 5;
    //the amount of force added to a tank at the centre of the explosion
    public float m_ExplosionForce = 100f;

    //Reference to the particles that will play on explosion
    public ParticleSystem m_ExplosionParticles;

    // Start is called before the first frame update
    private void Start()
    {
        //if it isn't destroyed by then, destroy the shell after it's lifetime
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void OnCollisionEnter(Collision other) 
    {
        //find the rigidbody of the collision object
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        //only tanks will have rigidbody scripts
        if (targetRigidbody != null)
        {
            //add an explosion force
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (targetHealth != null)
            {
                //Calculate the amount fo damage the target should take based on it's distance form the shell.
                float damage = CalculateDamage(targetRigidbody.position);

                //deal this damage to the tank
                targetHealth.TakeDamage(damage);
            }
        }

        //Unparent the particles from the sheell
        m_ExplosionParticles.transform.parent = null;

        //Play the particle system
        m_ExplosionParticles.Play();

        //Once the particles have finsihed, destroy the gameObject they are on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        //Destroy the shell
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        //Create a vector from the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;

        //Calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        //Calculate the proportion of the maximum distance (the explosionRadius)
        //the target is way
        float relativeDistance =
            (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //calculate damage as this proporition of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;

        //make suyre that the minimum damage is always 0
        damage = Mathf.Max(0f, damage);

        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
