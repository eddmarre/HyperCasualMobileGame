using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomMaterialColor : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private void Start()
    {
        Random random = new Random();
        var assign=random.Next(_materials.Length);
        GetComponent<MeshRenderer>().material = _materials[assign];
    }
}