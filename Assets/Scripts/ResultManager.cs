using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Text time;
    public Text score;  
    // Start is called before the first frame update
    void Start()
    {
        time.text = ((int)GameManager.time).ToString() + "sec";
        score.text = GameManager.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
