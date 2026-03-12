using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectPlot : MonoBehaviour
{
    public GameLogic GameLogic;
    public GameObject GameMaster;
    public bool availableP1 = false;
    public bool availableP2 = false;
    private Material original;
    private Material newMat;
    private Terrain terr;
    public int NumOfBuildings;
    public bool check0 = false, check1 = false, check2 = false;

    // Start is called before the first frame update
    void Start()
    {
        NumOfBuildings = 0;
        GameLogic = GameMaster.GetComponent<GameLogic>();
        terr = gameObject.GetComponent<Terrain>();
        original = terr.materialTemplate;
        newMat = Resources.Load("Material/Grass", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyPlot()
    {
        if (checkList() & GameLogic.selected)
        {
            int discount = 0;
            if (check0)
            {
                GameLogic.swapPlayer();
                GameLogic.move.ownedPlots.Remove(GameLogic.plotName);
                GameLogic.move.PlayerGold += 1000;
                GameLogic.swapPlayer();
                discount = 1000;
            }
            GameLogic.move.ownedPlots.Add(GameLogic.plotName);
            GameLogic.move.PlayerGold -= 2000 - discount;
            GameLogic.selected = false;
            GameLogic.checkGameOver();
            GameLogic.swapPlayer();
            GameLogic.RollDice();
        }
    }

    private void OnMouseDown() // select plot
    {
        bool available = GameLogic.move.gameObject.name == "Player1" ? availableP1 : availableP2;
        if (available & isNotMoving() & GameLogic.Action.Equals("BuyNode") || GameLogic.Action.Equals("BuildNode") || GameLogic.Action.Equals("FreeBuildNode"))
        {
            GameLogic.plotName = gameObject.name;
            GameObject.Find("PlotName").GetComponent<TextMeshProUGUI>().text = "Plot Name: " + gameObject.name;
            GameLogic.selected = true;
        }
    }

    public bool isNotMoving()
    {
        return GameLogic.started & GameLogic.move.walk.velocity == new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player1") availableP1 = true;
        if (other.name == "Player2") availableP2 = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player1") availableP1 = false;
        if (other.name == "Player2") availableP2 = false;
    }

    private void OnMouseEnter()
    {
        bool available = GameLogic.move.gameObject.name == "Player1" ? availableP1 : availableP2;
        if (available  & isNotMoving() & GameLogic.Action.Equals("BuyNode") || GameLogic.Action.Equals("BuildNode") || GameLogic.Action.Equals("FreeBuildNode"))
        {
            terr.materialTemplate = newMat;           
        }
    }

    private void OnMouseExit()
    {
        terr.materialTemplate = original;
    }

    private bool checkList()
    {
        check1 = !GameLogic.move.ownedPlots.Contains(GameLogic.plotName);
        GameLogic.swapPlayer();
        check0 = (GameLogic.move.ownedPlots.Contains(GameLogic.plotName) & (GameObject.Find(GameLogic.plotName).GetComponent<SelectPlot>().NumOfBuildings == 0));  // owned by Opponent but has no buildings
        check2 = !GameLogic.move.ownedPlots.Contains(GameLogic.plotName) || check0;
        GameLogic.swapPlayer();
        return check1 & check2;
    }

}
