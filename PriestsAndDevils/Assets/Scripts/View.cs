using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PriestsAndDevils;

public class View : MonoBehaviour
{
    // Start is called before the first frame update
    SSDirector ssd;
    void Start()
    {
        ssd = SSDirector.GetInstance();
    }
    public void OnGUI()
    {
        if (ssd.state == State.Win)
        {
            if (GUI.Button(new Rect(250, 400, 500, 50), "You Win! (click to restart)"))
            {
                ssd.Restart();
            }
        }
        else if(ssd.state == State.Lose)
        {
            if(GUI.Button(new Rect(250, 400, 500, 50), "You Lose! (click to restart)"))
            {
                ssd.Restart();
            }
        }
        else if(ssd.state == State.StopAtRight)
        {
            if(GUI.Button(new Rect(700, 200, 50, 50), "Priest"))
            {
                ssd.PriestOn();
            }
            if(GUI.Button(new Rect(800, 200, 50, 50), "Devil"))
            {
                ssd.DevilOn();
            }
            if(GUI.Button(new Rect(400, 400, 50, 50), "Move")){
                ssd.Move();
            }
            if(GUI.Button(new Rect(300, 100, 150, 50), "OFF_Left"))
            {
                ssd.Offleft();
            }
            if (GUI.Button(new Rect(500, 100, 150, 50), "OFF_Right"))
            {
                ssd.Offright();
            }
        }
        else if(ssd.state == State.StopAtLeft)
        {
            if (GUI.Button(new Rect(200, 200, 50, 50), "Priest"))
            {
                ssd.PriestOn();
            }
            if (GUI.Button(new Rect(100, 200, 50, 50), "Devil"))
            {
                ssd.DevilOn();
            }
            if (GUI.Button(new Rect(400, 400, 50, 50), "Move"))
            {
                ssd.Move();
            }
            if (GUI.Button(new Rect(300, 100, 150, 50), "OFF_Left"))
            {
                ssd.Offleft();
            }
            if (GUI.Button(new Rect(500, 100, 150, 50), "OFF_Right"))
            {
                ssd.Offright();
            }
        }
        else if(ssd.state == State.RightToLeft)
        {

        }
        else if(ssd.state== State.LeftToRight)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
