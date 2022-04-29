using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ScrollAlongZAxis : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool backWards = false;
    private float backOrForth = 1;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        if (backWards)
        {
            backOrForth = -1;
        }
    }

    private void Update()
    {
        _rigidBody.AddForce(Vector3.forward * speed * backOrForth);
    }
}