using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 10;
    public float lifespan = 10;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * -1f * speed * Time.deltaTime);
    }

    public void SetSortingOrder(int i)
    {
        GetComponent<SpriteRenderer>().sortingOrder = i;
    }
}
