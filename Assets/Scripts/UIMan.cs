using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMan : MonoBehaviour
{
    [SerializeField] Transform meta, robocik, player;
    [SerializeField] Slider hype;
    [SerializeField] float maxDist=250, minDist = 1;
    [SerializeField] TextMeshProUGUI TMP, year;
    bool toolate = false;
    Run r;
    private void Start()
    {
        r = FindObjectOfType<Run>();
        Vector2 metaV2 = new Vector2(meta.position.x, meta.position.z);
        Vector2 playerV2 = new Vector2(player.position.x, player.position.z);
        robocik.position = player.position;
        robocik.LookAt(meta);
        robocik.position += (robocik.forward * Data.dist);
        year.text = Data.year.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        TMP.text = r.powerUps.ToString();
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
    public void entered(string name)
    {
        if (toolate)
        {
            Data.year += Random.Range(1, 10);
            Vector2 robocikV2 = new Vector2(robocik.position.x, robocik.position.z);
            Vector2 playerV2 = new Vector2(player.position.x, player.position.z);
            Data.dist = Mathf.Abs(Vector2.Distance(robocikV2, playerV2));
            SceneManager.LoadScene(name.Split(',')[0]);
        }
        else
        {
            SceneManager.LoadScene(name.Split(',')[1]);
        }
    }
}

public static class Data{
    public static float year = 2020, dist=10;
}
