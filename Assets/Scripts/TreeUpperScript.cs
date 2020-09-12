using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TreeUpperScript : MonoBehaviour
{

    public bool touchedGround;
    private CinemachineImpulseSource impulseSource;
    public ParticleSystem smokeParticle;

    private void Start()
    {
        if (GetComponent<CinemachineImpulseSource>() != null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Ground"))
            return;

        if (!touchedGround)
        {
            touchedGround = true;
            if (impulseSource != null)
                impulseSource.GenerateImpulse();
            if (smokeParticle != null)
                smokeParticle.Play();
        }
    }


}
