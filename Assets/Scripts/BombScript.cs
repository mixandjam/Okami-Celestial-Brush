using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    public float explosionTime = 2;
    public GameObject explosionPrefab;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        StartCoroutine(ExplosionRoutine());

        IEnumerator ExplosionRoutine()
        {
            yield return new WaitForSeconds(explosionTime);
            impulseSource.GenerateImpulse();
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
