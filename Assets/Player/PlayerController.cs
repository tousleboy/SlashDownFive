using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform[] map = new Transform[9];
    public float speed = 10f;

    public Animator animator;

    public SpriteRenderer headr;
    public SpriteRenderer bodyr;
    public SpriteRenderer legr;

    public LayerMask layermask;

    [System.NonSerialized]
    public Vector2Int pos = new Vector2Int(1, 1);
    bool cooltime = false;

    public enum GAMESTATE
    {
        playing,
        gameclear,
        gameover
    }
    public static GAMESTATE gameState;

    AudioSource soundPlayer;
    public AudioClip shakin;
    public AudioClip bishi;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = map[PosToNum(pos)].position;
        OrderInLayerChange((pos.y + 1) * 4);
        gameState = GAMESTATE.playing;

        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cooltime);
        if(gameState != GAMESTATE.playing)
        {
            if(gameState == GAMESTATE.gameclear)
            {
                transform.Translate(Vector3.left * -5 * Time.deltaTime);
            }
            return;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            soundPlayer.PlayOneShot(shakin);
            animator.SetTrigger("attack");
        }
        if(cooltime) return;
        
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");

        bool up = Physics2D.Linecast(transform.position - (transform.right * 0.1f), transform.position - (transform.right * 0.1f) + (transform.up * 0.5f), layermask);
        up = up || Physics2D.Linecast(transform.position + (transform.right * 0.1f), transform.position + (transform.right * 0.1f) + (transform.up * 0.5f), layermask);
        bool down = Physics2D.Linecast(transform.position - (transform.right * 0.1f), transform.position - (transform.right * 0.1f) - (transform.up * 0.5f), layermask);
        down = down || Physics2D.Linecast(transform.position + (transform.right * 0.1f), transform.position + (transform.right * 0.1f) - (transform.up * 0.5f), layermask);
        bool left = Physics2D.Linecast(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) - (transform.right * 1f), layermask);
        left = left || Physics2D.Linecast(transform.position - (transform.up * 0.1f), transform.position - (transform.up * 0.1f) - (transform.right * 1f), layermask);
        bool right = Physics2D.Linecast(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (transform.right * 1f), layermask);
        right = right || Physics2D.Linecast(transform.position - (transform.up * 0.1f), transform.position - (transform.up * 0.1f) + (transform.right * 1f), layermask);
        Debug.Log("up " + up +"down " + down + "left " + left + "right " + right);

        if(up && axisV > 0 || down && axisV < 0 || left && axisH > 0 || right && axisH < 0) return;

        if(!(axisH == 0 && axisV == 0))
        {
            int x = pos.x;
            int y = pos.y;
            if(axisH > 0) x = Mathf.Min(pos.x + 1, 2);
            else if(axisH < 0) x = Mathf.Max(pos.x - 1, 0);

            if(axisV > 0) y = Mathf.Max(pos.y - 1, 0);
            else if(axisV < 0) y = Mathf.Min(pos.y + 1, 2);

            OrderInLayerChange((y + 1) * 4);

            pos = new Vector2Int(x, y);
            Debug.Log(pos);

            StartCoroutine(Move(map[PosToNum(pos)]));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            Debug.Log("hit");
            gameState = GAMESTATE.gameover;
            soundPlayer.PlayOneShot(bishi);
            StartCoroutine("Die");
        }
    }

    int PosToNum(Vector2Int position)
    {
        return position.x + position.y * 3;
    }

    void OrderInLayerChange(int o)
    {
        headr.sortingOrder = o;
        bodyr.sortingOrder = o - 1;
        legr.sortingOrder = o - 2;
    }

    IEnumerator Move(Transform d)
    {
        Vector3 original = transform.position;
        cooltime = true;
        float t = 0f;
        while(t < 1f)
        {
            if(gameState != GAMESTATE.playing) yield break;

            transform.position = Vector3.Lerp(original, d.position, t);
            t += 10 * Time.deltaTime;
            yield return null;
        }
        transform.position = d.position;
        yield return new WaitForSeconds(0.12f);
        cooltime = false;
    }
    IEnumerator Die()
    {
        while(true)
        {
            transform.Translate(Vector3.left * BackGroundManager.speed * 1.5f * Time.deltaTime);
            yield return null;
        }
    }
}
