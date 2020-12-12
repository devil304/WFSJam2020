using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFade : MonoBehaviour
{
    [SerializeField] AudioSource AS1, AS2;
    [SerializeField] AudioClip[] list;
    [SerializeField] float speed=1;
    private void Start()
    {
        AS1.clip = list[Random.Range(0, list.Length)];
        AS1.Play();
    }
    private void Update()
    {
        if (AS1.time>=AS1.clip.length-speed && AS2.clip==null)
        {
            int val = Random.Range(0, list.Length);
            while (list[val]==AS1.clip)
            {
                val = Random.Range(0, list.Length);
            }
            StartCoroutine(Transition(list[val]));
        }
    }

    IEnumerator Transition(AudioClip New)
    {
        float timed=0;
        AS2.clip = New;
        AS2.Play();
        while (AS2.volume < 1)
        {
            timed += Time.deltaTime;
            timed = timed > speed ? speed : timed;
            AS1.volume = Mathf.Clamp(Mathf.Abs(timed-speed) / speed,0,1);
            AS2.volume = Mathf.Clamp(timed / speed,0,1);
            yield return null;
        }
        AS1.clip = AS2.clip;
        AS1.Play();
        AS1.time = AS2.time;
        AS2.clip = null;
        AS1.volume = 1;
        AS2.volume = 0;
        AS2.Stop();
    }
}
