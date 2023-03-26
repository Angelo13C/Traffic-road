using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CopyCinemachinePivotFromTarget : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _cinemachineCamera;
    [SerializeField] private Transform _target;

    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        _cinemachineCamera.m_PivotOffset = _target.position;
    }
}
