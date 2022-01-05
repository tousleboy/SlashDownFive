using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashableObjectScript : MonoBehaviour
{
    public float speedrate = 1f;
    public int probability = 50;
    public bool justOneMove = true;
    Transform mainCamera;
    Renderer r;
    Vector3 originalPos;
    bool move = false;
    bool done = false;

    public GameObject top;
    Vector3 originalTopPos;

    AudioSource soundPlayer;
    public AudioClip kin;

    public static bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        r = GetComponent<Renderer>();
        originalPos = transform.position;
        if(justOneMove) probability = 100;
        StartCoroutine("Try");

        originalTopPos = top.transform.position - transform.position;

        stop = false;

        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(move && !(justOneMove && done) && !stop)
        {
            transform.Translate(Vector3.left * BackGroundManager.speed * Time.deltaTime);
            if(transform.position.x < mainCamera.position.x && !r.isVisible)
            {
                transform.position = originalPos;
                move = false;
                done = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            soundPlayer.PlayOneShot(kin);
            StartCoroutine("Cut");
        }
    }

    IEnumerator Try()
    {
        while(true)
        {
            if(!move)
            {
                if(Probability(probability)) move = true;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator Cut()
    {
        Renderer r = GetComponent<Renderer>();
        Vector3 direction = new Vector3(2f, -2f, 0);
        GameManager.score += 10;
        while(r.isVisible)
        {
            top.transform.Translate(direction * Time.deltaTime);
            yield return null;
        }
        top.transform.position = originalTopPos + transform.position;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    bool Probability(int p)
    {
        int r = Random.Range(1, 100);
        return p > r;
    }
}
