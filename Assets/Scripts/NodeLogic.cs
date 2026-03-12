using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeLogic : MonoBehaviour
{

    public GameLogic GameLogic;
    public GameObject GameMaster;
    public bool hasEntrance;
    public string Owner;
    public string plotOfEntrance;
    public List<string> AdjacentPlots;
    public int diceRoll = 0, cost = 0;

    // Start is called before the first frame update
    void Start()
    {
        Owner = "";
        plotOfEntrance = "";
        GameLogic = GameMaster.GetComponent<GameLogic>();
        hasEntrance = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseDown() // build Entrance
    {
        if (!(GameLogic.Action == "FreeEntranceNode" || GameLogic.move.BuildEntrancePoint)) return;
        foreach (string plot in AdjacentPlots)
        {
            if (!hasEntrance & GameLogic.move.ownedPlots.Contains(plot) & GameObject.Find(plot).GetComponent<SelectPlot>().NumOfBuildings > 0)
            {
                hasEntrance = true;
                Owner = GameLogic.move.gameObject.name;
                plotOfEntrance = plot;
                Instantiate(GameLogic.Entrance, new Vector3(this.transform.position.x, this.transform.position.y + 15, this.transform.position.z),
                    Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y + 90, this.transform.rotation.eulerAngles.z));
                if(!GameLogic.move.BuildEntrancePoint)
                {
                    GameLogic.checkGameOver();
                    GameLogic.swapPlayer();
                    GameLogic.RollDice();
                }
                else
                {
                    GameLogic.move.PlayerGold -= 1000;
                    GameLogic.move.BuildEntrancePoint = false;
                }
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int Rent = 150;
        if(other.tag == "Plot") AdjacentPlots.Add(other.gameObject.name);  // get all the adjacent plots
        GameLogic.Action = gameObject.tag;

        if (GameLogic.move.Steps == GameLogic.move.Roll - 1)  // activate UI when you reach target node
        {
            GameLogic.PlayerInfo.SetActive(true);
            if(!GameLogic.SinglePlayerMode)GameLogic.EnemyInfo.SetActive(true);
            if (hasEntrance & !(Owner == other.name))    // Landed on Entrance so PAY UP
            {
                if (plotOfEntrance.Contains("The Prancing Pony")) Rent = Rent * 2;         // Legendary Buildings only 2 but High Rent
                diceRoll = ((int)Random.Range(1f, 6f));
                cost = diceRoll * GameObject.Find(plotOfEntrance).GetComponent<SelectPlot>().NumOfBuildings * Rent;
                GameLogic.move.PlayerGold -= cost;
                GameLogic.swapPlayer();
                GameLogic.move.PlayerGold += cost;
                GameLogic.swapPlayer();
                GameLogic.message.GetComponent<TextMeshProUGUI>().text = "You stay for " + diceRoll + " nights and you pay: " + cost;
                Invoke("CloseMessage", 4f);
            }

            if (gameObject.tag == "BuyNode")
            {
                GameLogic.BuildPanel.gameObject.SetActive(false);
                GameLogic.BuyPanel.gameObject.SetActive(true);
                
            }
            else if (gameObject.tag == "BuildNode")
            {
                GameLogic.BuyPanel.gameObject.SetActive(false);
                GameLogic.BuildPanel.gameObject.SetActive(true);
                GameLogic.BuildButton.SetActive(true);
            }
            else if (gameObject.tag == "FreeBuildNode")
            {
                GameLogic.BuildEntranceMsg.SetActive(false);
                GameLogic.FreeBuildMessage.SetActive(true);
                GameLogic.BuyPanel.gameObject.SetActive(false);
                GameLogic.BuildPanel.gameObject.SetActive(true);
                GameLogic.BuildButton.SetActive(true);
                Invoke("CloseMessage", 4f);
            }
            else if (gameObject.tag == "FreeEntranceNode")
            {               
                GameLogic.FreeBuildMessage.SetActive(false);
                GameLogic.BuildEntranceMsg.SetActive(true);
                GameLogic.BuyPanel.gameObject.SetActive(false);
                GameLogic.BuildPanel.gameObject.SetActive(true);
                GameLogic.BuildButton.SetActive(false);
                Invoke("CloseMessage", 4f);
            }
        }
    }

    private void CloseMessage()
    {
        GameLogic.FreeBuildMessage.SetActive(false);
        GameLogic.BuildEntranceMsg.SetActive(false);
        GameLogic.message.GetComponent<TextMeshProUGUI>().text = "";
    }
}
