using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PriestsAndDevils;

public class Model : MonoBehaviour
{
    Stack<GameObject> RightPriests = new Stack<GameObject>();
    Stack<GameObject> RightDevils = new Stack<GameObject>();
    Stack<GameObject> LeftPriests = new Stack<GameObject>();
    Stack<GameObject> LeftDevils = new Stack<GameObject>();
    Stack<GameObject> Boat = new Stack<GameObject>();
    SSDirector ssd;
    GameObject Boat_obj;
    // Start is called before the first frame update
    void Start()
    {
        ssd = SSDirector.GetInstance();
        ssd.SetModel(this);
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        Correct(RightPriests, new Vector3(4.6f, 0.8f, 10));
        Correct(RightDevils, new Vector3(6.2f, 0.8f, 10));
        Correct(LeftPriests, new Vector3(-5.6f, 0.8f, 10));
        Correct(LeftDevils, new Vector3(-7.6f, 0.8f, 10));
        if (ssd.state != State.RightToLeft && ssd.state != State.LeftToRight)
        {
            if (ssd.state == State.StopAtRight)
            {
                Correct(Boat, new Vector3(3, 0.8f, 10));
            }
            if (ssd.state == State.StopAtLeft)
            {
                Correct(Boat, new Vector3(-3, 0.8f, 10));
            }
        }
        if (ssd.state == State.RightToLeft)
        {
            Boat_obj.transform.position = Vector3.MoveTowards(Boat_obj.transform.position, new Vector3(-3, 0, 10), 10f * Time.deltaTime);
            for(int i = 0; i < Boat.Count; i++)
            {
                Boat.ToArray()[i].transform.position= Vector3.MoveTowards(Boat.ToArray()[i].transform.position, new Vector3(-3 + i*0.6f, 0.8f, 10), 10f * Time.deltaTime);
            }
            if(Boat_obj.transform.position==new Vector3(-3, 0, 10))
            {
                ssd.state = State.StopAtLeft;
                while (Boat.Count != 0)
                {
                    GameObject t = Boat.Pop();
                    Debug.Log(t.name);
                    if (t.name == "Priest(Clone)")
                    {
                        LeftPriests.Push(t);
                    }
                    else if (t.name == "Devil(Clone)")
                    {
                        LeftDevils.Push(t);
                    }
                }
                Check();
            }
        }
        else if (ssd.state == State.LeftToRight)
        {
            Boat_obj.transform.position = Vector3.MoveTowards(Boat_obj.transform.position, new Vector3(3, 0, 10), 10f * Time.deltaTime);
            for (int i = 0; i < Boat.Count; i++)
            {
                Boat.ToArray()[i].transform.position = Vector3.MoveTowards(Boat.ToArray()[i].transform.position, new Vector3(3 + i * 0.6f, 0.8f, 10), 10f * Time.deltaTime);
            }
            if (Boat_obj.transform.position == new Vector3(3, 0, 10))
            {
                ssd.state = State.StopAtRight;
                while (Boat.Count != 0)
                {
                    GameObject t = Boat.Pop();
                    Debug.Log(t.name);
                    if (t.name == "Priest(Clone)")
                    {
                        RightPriests.Push(t);
                    }
                    else if (t.name == "Devil(Clone)")
                    {
                        RightDevils.Push(t);
                    }
                }
                Check();
            }
        }
    }
    void Build()
    {
        Instantiate(Resources.Load("Prefabs/Bank"), new Vector3(6, 0, 10), Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Bank"), new Vector3(-6, 0, 10), Quaternion.identity);

        Boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), new Vector3(3, 0, 10), Quaternion.identity) as GameObject;

        for (int i = 0; i < 3; i++)
        {
            RightPriests.Push(Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);
            RightDevils.Push(Instantiate(Resources.Load("Prefabs/Devil")) as GameObject);
        }
    }
    void Correct(Stack<GameObject> gos,Vector3 pos)  //这里参照了网上的定位方法
    {
        for(int i = gos.Count - 1; i >= 0; i--)
        {
            gos.ToArray()[i].transform.position = pos + new Vector3(0.6f * (gos.Count - 1 - i), 0, 0);
        }
    }
    public void PriestOn()
    {
        if (ssd.state == State.StopAtRight)
        {
            if (RightPriests.Count != 0 && Boat.Count < 2)
            {
                Boat.Push(RightPriests.Pop());
            }
        }
        else if(ssd.state == State.StopAtLeft)
        {
            if (LeftPriests.Count != 0 && Boat.Count < 2)
            {
                Boat.Push(LeftPriests.Pop());
            }
        }
    }
    public void DevilOn()
    {
        if (ssd.state == State.StopAtRight)
        {
            if (RightDevils.Count != 0 && Boat.Count < 2)
            {
                Boat.Push(RightDevils.Pop());
            }
        }
        else if (ssd.state == State.StopAtLeft)
        {
            if (LeftDevils.Count != 0 && Boat.Count < 2)
            {
                Boat.Push(LeftDevils.Pop());
            }
        }
    }
    public void Move()
    {
        if (ssd.state == State.StopAtRight && Boat.Count != 0)
        {
            ssd.state = State.RightToLeft;
        }
        if (ssd.state == State.StopAtLeft && Boat.Count != 0)
        {
            ssd.state = State.LeftToRight;
        }
    }
    public void Check()
    {
        if ((LeftPriests.Count!=0&&LeftDevils.Count > LeftPriests.Count) || (RightDevils.Count > RightPriests.Count&&RightPriests.Count!=0))
        {
            ssd.state = State.Lose;
        }
        else if (LeftPriests.Count == 3 && LeftDevils.Count == 3)
        {
            ssd.state = State.Win;
        }
    }
    public void Restart()
    {
        ssd.state = State.StopAtRight;
        while (Boat.Count != 0)
        {
            GameObject t = Boat.Pop();
            Debug.Log(t.name);
            if (t.name == "Priest(Clone)")
            {
                RightPriests.Push(t);
            }
            else if (t.name == "Devil(Clone)")
            {
                RightDevils.Push(t);
            }
        }
        while (LeftPriests.Count != 0)
        {
            RightPriests.Push(LeftPriests.Pop());
        }
        while (LeftDevils.Count != 0)
        {
            RightDevils.Push(LeftDevils.Pop());
        }
        Boat_obj.transform.position = new Vector3(3, 0, 10);
    }
    public void Offleft()
    {
        if (Boat.Count >= 1)
        {
            if (Boat.Count == 2)
            {
                GameObject t1 = Boat.Pop();
                GameObject t2 = Boat.Pop();
                if (t2.name == "Devil(Clone)")
                {
                    if (ssd.state == State.StopAtRight)
                    {
                        RightDevils.Push(t2);
                    }
                    else
                    {
                        LeftDevils.Push(t2);
                    }
                }
                else if (t2.name == "Priest(Clone)")
                {
                    if (ssd.state == State.StopAtRight)
                    {
                        RightPriests.Push(t2);
                    }
                    else
                    {
                        LeftPriests.Push(t2);
                    }
                }
                Boat.Push(t1);
            }
            else
            {
                GameObject t = Boat.Pop();
                if (t.name == "Devil(Clone)")
                {
                    if (ssd.state == State.StopAtRight)
                    {
                        RightDevils.Push(t);
                    }
                    else
                    {
                        LeftDevils.Push(t);
                    }
                }
                else if (t.name == "Priest(Clone)")
                {
                    if (ssd.state == State.StopAtRight)
                    {
                        RightPriests.Push(t);
                    }
                    else
                    {
                        LeftPriests.Push(t);
                    }
                }
            }
        }
    }
    public void Offright()
    {
        if (Boat.Count == 2)
        {
            GameObject t = Boat.Pop();
            if (t.name == "Devil(Clone)")
            {
                if (ssd.state == State.StopAtRight)
                {
                    RightDevils.Push(t);
                }
                else
                {
                    LeftDevils.Push(t);
                }
            }
            else if (t.name == "Priest(Clone)")
            {
                if (ssd.state == State.StopAtRight)
                {
                    RightPriests.Push(t);
                }
                else
                {
                    LeftPriests.Push(t);
                }
            }
        }
    }
}
