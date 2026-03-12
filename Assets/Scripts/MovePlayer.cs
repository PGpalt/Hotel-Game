using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public GameObject node;
    public GameObject GameMaster;
    public GameLogic GameLogic;
    public int nodeNum;
    public Animator walk;
    public bool MyTurn;
    public int Roll;
    public int Steps;
    public GameObject PlayerCam;
    public int PlayerGold;
    public List<string> ownedPlots;
    public bool BuildEntrancePoint;



    // Start is called before the first frame update
    void Start()
    {
        BuildEntrancePoint = false;
        PlayerGold = 10000;
        PlayerCam.gameObject.SetActive(false);
        MyTurn = false ;
        nodeNum = 0;
        walk = gameObject.GetComponent<Animator>();
        GameLogic = GameMaster.GetComponent<GameLogic>();
        Steps = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorMove()
    {
        if (MyTurn & Steps < Roll)
        {
            node = GameObject.Find("Node" + nodeNum);
            float speed = 70f;
            float step = speed * Time.deltaTime; // calculate distance to move

            walk.Play("Male Sword Sprint");

            transform.rotation = Quaternion.RotateTowards(transform.rotation, node.transform.rotation, step);
            transform.position = Vector3.MoveTowards(transform.position, node.transform.position, step);

            if (transform.position == node.transform.position)
            {
                nodeNum++;
                Steps++;
                if (nodeNum == GameLogic.LastNode) nodeNum = 0;
                if (Steps == Roll) GameLogic.SwapCamToMain();
            }
        }
        else
        {
            Steps = 0;
            MyTurn = false;
            walk.Play("Male Sword Stance");
        }
    }

}
