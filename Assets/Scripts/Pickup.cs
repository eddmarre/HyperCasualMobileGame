using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pickup : MonoBehaviour
{
    private IObjectPool<GameObject> _pickUpObjectPool;


    public void SetKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void DropPickUp()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BulletDestroyer"))
            Destroy(gameObject);
    }
}