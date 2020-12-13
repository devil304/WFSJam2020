using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAfterTime : MonoBehaviour
{
    [SerializeField] string Sname;
    [SerializeField] float time = 5;
    [SerializeField] TextMeshProUGUI year;
    // Start is called before the first frame update
    void Start()
    {
        year.text = Data.year.ToString();
        StartCoroutine(Back());
    }

    IEnumerator Back()
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(Sname);
    }
}
