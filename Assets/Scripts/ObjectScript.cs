using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public float speedrate = 1f;
    public int probability = 50;
    public bool justOneMove = true;
    Transform mainCamera;
    Renderer r;
    Vector3 originalPos;
    bool move = false;
    bool done = false;

    public static bool stop = false;
    // Start is called before the first frame update
    protected void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        r = GetComponent<Renderer>();
        originalPos = transform.position;
        if(justOneMove) probability = 100;

        StartCoroutine("Try");

        stop = false;

    }

    // Update is called once per frame
    protected void Update()
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

    bool Probability(int p)
    {
        int r = Random.Range(1, 100);
        return p > r;
    }
}
