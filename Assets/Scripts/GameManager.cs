using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum EVENT
    {
        nothing,
        hidegrid,
        showgrid,
        showwasd,
        showattackrange1,
        showattackrange2,
        enemyappear,
        newability
    }

    public static EVENT nowevent;
    static EVENT pastevent;
    public static bool enemyLose = false;

    public GameObject[] enemies;
    public GameObject[] sei = new GameObject[5];
    public Image wasd;
    public Image j;
    public GameObject panel;
    public GameObject brade;
    public SpriteRenderer attackRange1;
    public SpriteRenderer attackrange2;
    public GameObject ear1;
    public GameObject ear2;
    public GameObject effect;
    public GameObject fade;

    public static int enemynum = 0;

    AudioSource soundPlayer;
    public AudioClip quiin;
    public AudioClip deen;

    public AudioSource musicPlayer;
    ChangeScene cs;

    public static int score;
    public static float time = 0;

    public Transform[] RespawnPos;
    public GameObject world;

    // Start is called before the first frame update
    void Awake()
    {
        world.transform.position = RespawnPos[(int)Mathf.Min(enemynum, 2)].position;
        nowevent = EVENT.nothing;
        pastevent = EVENT.nothing;

        panel.SetActive(false);

        soundPlayer = GetComponent<AudioSource>();
        cs = GetComponent<ChangeScene>();

        int len = sei.Length;
        int i;
        for(i = enemynum; i < len; i++) sei[i].SetActive(false);

        len = enemies.Length;
        for(i = 0; i < len; i++) enemies[i].SetActive(false);

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.gameState == PlayerController.GAMESTATE.playing)
        {

            time += Time.deltaTime;

            if(nowevent != pastevent)
            {
                if(nowevent == EVENT.enemyappear)
                {
                    ObjectScript.stop = true;
                    SlashableObjectScript.stop = true;
                    enemies[enemynum].SetActive(true);
                }
                else if(nowevent == EVENT.showwasd)
                {
                    StartCoroutine("ShowWASD");
                }
                else if(nowevent == EVENT.showattackrange1)
                {
                    StartCoroutine("ShowAttackrange1");
                }
                else if(nowevent == EVENT.newability)
                {
                    ear1.SetActive(true);
                    ear2.SetActive(true);
                    effect.SetActive(true);
                    StartCoroutine("Brade");
                    nowevent = EVENT.nothing;
                }
                pastevent = nowevent;
            }
            if(nowevent == EVENT.enemyappear && enemyLose)
            {
                enemyLose = false;
                ObjectScript.stop = false;
                SlashableObjectScript.stop = false;

                nowevent = EVENT.nothing;
                pastevent = EVENT.nothing;

                StartCoroutine(Sei(enemynum));
                enemynum += 1;
                if(enemynum >= enemies.Length)
                {
                    PlayerController.gameState = PlayerController.GAMESTATE.gameclear;
                    StartCoroutine("TurnOff");
                }
            }
        }
        else
        {
            if(PlayerController.gameState == PlayerController.GAMESTATE.gameover)
            {
                Invoke("GameOver", 1.0f);
            }
        }
    }

    void GameOver()
    {
        BackGroundManager.speed = 0f;
        panel.SetActive(true);
    }

    IEnumerator ShowAttackrange1()
    {
        int i;
        float t;
        float speed = 2.0f;
        for(i = 0; i < 3; i++)
        {
            t = 0f;
            while(t < 1)
            {
                j.color = Color.Lerp(Color.clear, Color.white, t);
                if(attackRange1 != null) attackRange1.color = Color.Lerp(Color.clear, Color.white, t);

                t += speed * Time.deltaTime;
                yield return null;
            }
            j.color = Color.white;
            t = 0f;
            while(t < 1)
            {
                j.color = Color.Lerp(Color.white, Color.clear, t);
                if(attackRange1 != null )attackRange1.color = Color.Lerp(Color.white, Color.clear, t);

                t += speed * Time.deltaTime;
                yield return null;
            }
            j.color = Color.clear;
        }

        nowevent = EVENT.nothing;
        pastevent = EVENT.nothing;
    }

    IEnumerator ShowWASD()
    {
        int i;
        float t;
        float speed = 2.0f;
        for(i = 0; i < 3; i++)
        {
            t = 0f;
            while(t < 1)
            {
                wasd.color = Color.Lerp(Color.clear, Color.white, t);
                t += speed * Time.deltaTime;
                yield return null;
            }
            wasd.color = Color.white;
            t = 0f;
            while(t < 1)
            {
                wasd.color = Color.Lerp(Color.white, Color.clear, t);
                t += speed * Time.deltaTime;
                yield return null;
            }
            wasd.color = Color.clear;
        }

        nowevent = EVENT.nothing;
        pastevent = EVENT.nothing;
    }

    IEnumerator Sei(int n)
    {
        float t = 3f;
        sei[n].transform.localScale = new Vector3(t, t, 1);
        sei[n].SetActive(true);
        float speed = 1.5f;

        soundPlayer.PlayOneShot(deen);

        while(t > 1)
        {
            sei[n].transform.localScale = new Vector3(t, t, 1);
            t -= speed * Time.deltaTime;
            yield return null;
        }
        sei[n].transform.localScale = new Vector3(1, 1, 1);
        BackGroundManager.speed *= 1.2f;
    }

    IEnumerator TurnOff()
    {
        float t = 0.0f;
        float speed = 0.5f;
        //Button bt = button.GetComponent<Button>();
        //bt.interactable = false;
        fade.SetActive(true);
        Image I = fade.GetComponent<Image>();
        yield return new WaitForSeconds(1.0f);
        while(t <= 1.0f)
        {
            I.color = Color.Lerp(Color.clear, Color.black, t);
            musicPlayer.volume = 1.0f - t; 
            t += speed * Time.deltaTime;
            yield return null;
        }
        I.color = Color.black;
        musicPlayer.volume = 0f;
        yield return new WaitForSeconds(1.0f);
        cs.Load();
    }

    IEnumerator Brade()
    {
        Image I = brade.GetComponent<Image>();
        Color c = I.color;

        soundPlayer.PlayOneShot(quiin);

        int i;
        for(i = 0; i < 5; i++)
        {
            I.color = c;
            yield return new WaitForSeconds(0.1f);
            I.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
