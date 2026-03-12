using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildEntrance : MonoBehaviour
{
    public GameObject GameMaster;
    public GameLogic GameLogic;


    // Start is called before the first frame update
    void Start()
    {
        GameLogic = GameMaster.GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            GameMaster.GetComponent<GameLogic>().move.BuildEntrancePoint = true;
            GameLogic.message.GetComponent<TextMeshProUGUI>().text = "You just passed the buy Entrance point!\n Click on a node to buy an Entrance.";
            Invoke("closeMsg", 5f);
        }
    }

    private void closeMsg()
    {
        GameLogic.message.GetComponent<TextMeshProUGUI>().text = "";
    }

}
