using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    private static CameraShakeManager _Instance;

    public static CameraShakeManager Instance
    {
        get { return _Instance; }
    }

    [SerializeField]
    private float m_GlobalShakeForce = 1.0f;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(m_GlobalShakeForce);
    }
}
