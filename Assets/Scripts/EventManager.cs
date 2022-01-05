using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameManager.EVENT gameEvent;
    Transform mainCamera;
    Renderer r;
    bool activated;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        r = GetComponent<Renderer>();
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(r.isVisible && !activated && transform.position.x > mainCamera.transform.position.x)
        {
            GameManager.nowevent = gameEvent;
            activated = true;
        }
    }
}
