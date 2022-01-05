using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Image back;
    public GameObject button;
    AudioSource soundPlayer;
    public AudioClip shakin; 
    // Start is called before the first frame update
    void Start()
    {
        GameManager.enemynum = 0;
        GameManager.time = 0f;
        soundPlayer = GetComponent<AudioSource>();

        StartCoroutine("TurnOn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TurnOn()
    {
        float t = 0;
        float speed = 0.8f;
        float limit = 0.6f;
        back.color = Color.clear;
        button.SetActive(false);

        soundPlayer.PlayOneShot(shakin);

        while(t <= limit)
        {
            back.color = Color.Lerp(Color.clear, Color.white, t);
            t += speed * Time.deltaTime;
            yield return null;
        }
        back.color = Color.Lerp(Color.clear, Color.white, limit);

        yield return new WaitForSeconds(1.0f);

        button.SetActive(true);

    }
}
