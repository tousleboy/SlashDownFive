using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] map = new Transform[9];
    PlayerController pc;
    public bool shoot = false;
    public bool shootBeam = false;
    Vector2Int pos;
    SpriteRenderer sr;
    BoxCollider2D bc;

    public GameObject bullet;
    public GameObject beam;
    public Transform gate;

    AudioSource soundPlayer;
    public AudioClip shun;
    public AudioClip shot;
    public AudioClip kin;
    // Start is called before the first frame update
    void Start()
    {
        /*pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        PosDecide();

        sr = GetComponent<SpriteRenderer>();
        //sr.color = Color.clear;

        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;

        StartCoroutine("Move");*/
    }

    void OnEnable()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        PosDecide();

        sr = GetComponent<SpriteRenderer>();
        //sr.color = Color.clear;

        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;

        soundPlayer = GetComponent<AudioSource>();

        StartCoroutine("Move");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerAttack")
        {
            StartCoroutine("Die");
            Destroy(gameObject, 2.0f);
        }
    }

    int PosToNum(Vector2Int position)
    {
        return position.x + position.y * 3;
    }

    IEnumerator Move()
    {
        yield return null;
        float t = 0f;
        float speed = 2.5f;
        while(true)
        {
            PosDecide();
            transform.position = map[PosToNum(pos)].position;
            sr.sortingOrder = pos.y * 4 + 2;

            soundPlayer.PlayOneShot(shun);

            while(t <= 1.0f)
            {
                sr.color = Color.Lerp(Color.clear, Color.white, t);
                t += speed * Time.deltaTime;
                yield return null;
            }
            t = 0f;
            sr.color = Color.white;
            bc.enabled = true;

            if(shoot)
            {
                if(shootBeam && Probability(100))
                {
                    yield return new WaitForSeconds(0.5f);
                    Shoot(beam);
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                    Shoot(bullet);
                    yield return new WaitForSeconds(1.0f);
                }
            }
            else 
            {
                yield return new WaitForSeconds(1.0f);
            }

            bc.enabled = false;

            soundPlayer.PlayOneShot(shun);

            while(t <= 1.0f)
            {
                sr.color = Color.Lerp(Color.white, Color.clear, t);
                t += speed * Time.deltaTime;
                yield return null;
            }
            t = 0f;
            sr.color = Color.clear;

            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator Die()
    {
        bc.enabled = false;
        GameManager.enemyLose = true;
        GameManager.score += 100;

        soundPlayer.PlayOneShot(kin);

        while(sr.isVisible)
        {
            transform.Translate(Vector3.left * BackGroundManager.speed * 1.5f * Time.deltaTime);
            yield return null;
        }
    }

    void PosDecide()
    {
        do
        {
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);
            pos = new Vector2Int(x, y);
        }while(pos == pc.pos);
    }

    void Shoot(GameObject bullet)
    {
        Debug.Log("shoot");

        soundPlayer.PlayOneShot(shot);

        GameObject obj = Instantiate(bullet, gate.position, Quaternion.identity);
        obj.GetComponent<EnemyBulletScript>().SetSortingOrder(sr.sortingOrder);
    }

    bool Probability(int p)
    {
        int r = Random.Range(1, 101);
        return p >= r;
    }
}
