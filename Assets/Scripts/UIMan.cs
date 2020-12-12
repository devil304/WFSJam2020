using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMan : MonoBehaviour
{
    [SerializeField] Transform meta, robocik, player;
    [SerializeField] Slider hype;
    [SerializeField] float maxDist=250, minDist = 1;
    bool toolate = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 metaV2 = new Vector2(meta.position.x,meta.position.z);
        Vector2 robocikV2 = new Vector2(robocik.position.x, robocik.position.z);
        Vector2 playerV2 = new Vector2(player.position.x, player.position.z);
        if (Mathf.Abs(Vector2.Distance(robocikV2, metaV2)) < minDist)
            toolate = true;
        if (Mathf.Abs(Vector2.Distance(playerV2, metaV2))> Mathf.Abs(Vector2.Distance(robocikV2, metaV2))||toolate)
        {
            hype.value = Mathf.Clamp(Mathf.Abs(Mathf.Abs(Vector2.Distance(playerV2, robocikV2)-250) /250),0,1);
        }
        else
        {
            hype.value = hype.maxValue;
        }
    }
}
