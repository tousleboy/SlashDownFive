using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public GameObject[] objects;
    public Transform exit;
    public static float speed;
    int len;
    float eps = 1f;
    // Start is called before the first frame update
    void Start()
    {
        len = objects.Length;
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if(len == 0) return;

        int i;
        for(i = 0; i < len; i++)
        {
            objects[i].transform.Translate(Vector3.left * speed * Time.deltaTime);
            if(Mathf.Abs(objects[i].transform.position.x - exit.position.x) < eps)
            {
                objects[i].transform.position = new Vector3(transform.position.x, objects[i].transform.position.y, 0f);
            }
        }
    }
}
