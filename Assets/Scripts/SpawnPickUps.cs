using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Random = System.Random;

public class SpawnPickUps : MonoBehaviour
{
    [SerializeField] private GameObject pickUpItems;
    [SerializeField] private GameObject dropItem;
    [SerializeField] private float waveTime = 5f;

    public IObjectPool<GameObject> pickUpobjectPool;
    private IObjectPool<GameObject> dropObjectPool;

    private GameObject pickUpAdd, pickUpDrop;

    private float waveReset;

    private void Awake()
    {
        pickUpobjectPool =
            new ObjectPool<GameObject>(CreatePickups, onGetPool, onReturnPool, onDestroyPool, true, 10, 20);
        dropObjectPool = new ObjectPool<GameObject>(CreateDrop, onGetPool, onReturnPool, onDestroyPool, true, 10, 20);
    }

    private GameObject CreateDrop()
    {
        var pickUp = Instantiate(dropItem, transform.position, quaternion.identity);
        return pickUp;
    }

    private void Start()
    {
        pickUpobjectPool.Get();
        pickUpobjectPool.Get();
        dropObjectPool.Get();
        waveReset = waveTime;
    }

    private void Update()
    {
        if (waveTime > 0)
        {
            waveTime -= Time.deltaTime;
        }
        else if (waveTime <= 0)
        {
            pickUpobjectPool.Get();
            pickUpobjectPool.Get();
            dropObjectPool.Get();
            waveTime = waveReset;
        }
        else
        {
            waveTime = 0;
        }
    }

    private void onDestroyPool(GameObject obj)
    {
        Destroy(obj);
    }

    private void onReturnPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void onGetPool(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private GameObject CreatePickups()
    {
        var pickUp = Instantiate(pickUpItems, CreateRandomPosition(), quaternion.identity);
        return pickUp;
    }

    private Vector3 CreateRandomPosition()
    {
        var randomXPosition = UnityEngine.Random.Range(-5, 5f);
        var xOffset = new Vector3(randomXPosition, 0, 0);
        return transform.position + xOffset;
    }
    
}