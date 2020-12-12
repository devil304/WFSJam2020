using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triger : MonoBehaviour
{
    [SerializeField] UnityEvent UE;
    private void OnTriggerEnter(Collider other)
    {
        UE.Invoke();
    }
}
