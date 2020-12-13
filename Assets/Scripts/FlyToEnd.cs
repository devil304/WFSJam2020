using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToEnd : MonoBehaviour
{
    [SerializeField] float speed=1;
    private void Start()
    {
        //transform.LookAt(target.position);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
