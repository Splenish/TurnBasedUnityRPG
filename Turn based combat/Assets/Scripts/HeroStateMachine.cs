using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    public BaseHero hero;

    public enum TurnState
    {
        Processing,
        Addtolist,
        Waiting,
        Selecting,
        Action,
        Dead
    }

    public TurnState currentState;

    //ProgressBarille muuttujia
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;

    void Start()
    {
        currentState = TurnState.Processing;
    }

    void Update()
    {
        Debug.Log(currentState);
        switch(currentState)
        {
            case (TurnState.Processing):
                UpgradeProgressBar();
                break;
            case (TurnState.Addtolist):

                break;
            case (TurnState.Waiting):

                break;
            case (TurnState.Selecting):

                break;
            case (TurnState.Action):

                break;
            case (TurnState.Dead):

                break;
        }

    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 0.5274734f), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.Addtolist;
        }

    }


}
