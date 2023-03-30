

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameObjectCamera : MonoBehaviour
{
    public static Transform Instance;

    void Awake()
    {
        Instance = transform;
    }
}