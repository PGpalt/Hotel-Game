using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameLogic : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public MovePlayer move;
    public GameObject MainCam;
    public GameObject rollButton;
    public GameObject BuildPanel;
    public GameObject BuyPanel;
    public GameObject PlayerMoneyText;
    public GameObject plots_Owned;
    public TextMeshProUGUI PlayerMoney;
    public TextMeshProUGUI plots_Owned_Text;
    public string Action;
    public bool started;
    public string plotName;
    public bool selected;
    public List<GameObject> Buildings;
    public List<GameObject> Plots;
    public GameObject gameOverPanel;
    public GameObject message;
    public GameObject rollResult;
    public GameObject GameOverSoundEffect;
    public GameObject AmbientMusic;
    public GameObject FreeBuildMessage;
    public GameObject BuildEntranceMsg;
    public GameObject BuildEntrance;
    public GameObject BuildButton;
    public GameObject Entrance;
    public GameObject playerName;
    public GameObject playerWon;
    public GameObject EnemyPlotsText;
    public GameObject EnemyInfo;
    public GameObject PlayerInfo;
    public GameObject singleplayerNodes;
    public GameObject multiplayerNodes;
    public GameObject MainMenu;
    public GameObject exitToMainMenu;
    public Texture2D cursor;
    public bool SinglePlayerMode;
    public int LastNode;
    public GameObject rollDiceEnviroment;
    public GameObject buildDice;
    public GameObject normalDice;
    public GameObject Credits;
    public GameObject MenuButtons;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        MainMenu.SetActive(true);
        SinglePlayerMode = false;
        foreach(GameObject obj in Buildings)
        {
            obj.SetActive(false);
        }
        rollButton = GameObject.Find("Roll_Dice_Button");
        BuildPanel = GameObject.Find("BuildPanel");
        BuyPanel = GameObject.Find("BuyPanel");
        BuildPanel.gameObject.SetActive(false);
        BuyPanel.gameObject.SetActive(false);
        PlayerMoneyText = GameObject.Find("PlayerMoney");
        plots_Owned = GameObject.Find("OwnedPlots");
        PlayerMoney = PlayerMoneyText.GetComponent<TextMeshProUGUI>();
        plots_Owned_Text = plots_Owned.GetComponent<TextMeshProUGUI>();
        started = false;
        move = player1.GetComponent<MovePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitToMainMenu.SetActive(!exitToMainMenu.activeSelf);
        }
        PlayerMoney.text = "Player Gold: " + move.PlayerGold.ToString();
        plots_Owned_Text.text = "Plots Owned: ";
        EnemyPlotsText.GetComponent<TextMeshProUGUI>().text = "Enemy plots Owned: ";
        if (move.ownedPlots.Count != 0)
        {      
            foreach (string plot in move.ownedPlots)
                plots_Owned_Text.text += "\n " + plot;
        }
        swapPlayer();
        if (move.ownedPlots.Count != 0)
        {
            foreach (string plot in move.ownedPlots)
                EnemyPlotsText.GetComponent<TextMeshProUGUI>().text += "\n " + plot;
        }
        swapPlayer();
        playerName.GetComponent<TextMeshProUGUI>().text = move.gameObject.name;
    }
    public void multiPlayerMode()
    {
        MainMenu.SetActive(false);
        LastNode = 25;
    }
    public void singlePlayerMode()
    {
        LastNode = 16;
        SinglePlayerMode = true;
        player2.SetActive(false);
        BuildEntrance.SetActive(false);
        EnemyInfo.SetActive(false);
        playerWon.SetActive(false);
        multiplayerNodes.SetActive(false);
        singleplayerNodes.SetActive(true);
        MainMenu.SetActive(false);
        player2 = player1;
    }

    public void RollDice()
    {
        if ( move.walk.velocity == new Vector3(0, 0, 0))
        {
            started = true;
            rollButton.gameObject.SetActive(false);    
            GameObject.Find("PlotName").GetComponent<TextMeshProUGUI>().text = "";
            BuildPanel.SetActive(false);
            BuyPanel.SetActive(false);
            PlayerInfo.SetActive(false);
            EnemyInfo.SetActive(false);
            roll_3d_dice();
            plotName = "";
            move.BuildEntrancePoint = false;
        }
    }

    public void roll_3d_dice()
    {
        rollDiceEnviroment.SetActive(true);
        MainCam.SetActive(false);
        move.PlayerCam.SetActive(false);
    }

    public void checkGameOver()
    {
        if (move.PlayerGold < 0) // game over!
        {
            gameOverPanel.SetActive(true);
            AmbientMusic.SetActive(false);
            GameOverSoundEffect.SetActive(true);
            swapPlayer();
            playerWon.GetComponent<TextMeshProUGUI>().text = move.gameObject.name + " won!";
        }

    }

    public void endTurn()
    {
        checkGameOver();
        swapPlayer();
        RollDice();
    }

    public void swapPlayer()
    {
        if (move == player1.GetComponent<MovePlayer>())
        {
            move = player2.GetComponent<MovePlayer>();
        }
        else
        {
            move = player1.GetComponent<MovePlayer>();
        }
    }
    public void SwapCamToMain()
    {          
        move.PlayerCam.SetActive(false);  
        MainCam.SetActive(true);
    }

    public void SwapCamToPlayer()
    {
        MainCam.SetActive(false);
        move.PlayerCam.SetActive(true);
    }

    public void Build()
    {

        int factor = 1;
        if(Action == "BuildNode")
        {
            string result = buildDice.GetComponent<BuildDice>().side;

            if (result == "H")
            {
                factor = 0;
            }
            else if (result == "D")
            {
                factor = 2;
            }
            else if (result == "Red")
            {
                return;
            }
        }
        else
        {
            factor = 0; // Free Build Node
        }
        

        foreach(GameObject Obj in Buildings){
            if(Obj.name.Contains(plotName) && GameObject.Find(plotName).GetComponent<SelectPlot>().NumOfBuildings < 4)
            {
                GameObject.Find(plotName).GetComponent<SelectPlot>().NumOfBuildings++;
                Obj.SetActive(true);
                move.PlayerGold -= 1500 * factor;
                Buildings.Remove(Obj);
                break;
            }
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
    public void RollBuildDice()
    {
        if (plotName == "" || !move.ownedPlots.Contains(plotName) || GameObject.Find(plotName).GetComponent<SelectPlot>().NumOfBuildings == 4) return;
        if (plotName.Equals("The Prancing Pony") & GameObject.Find(plotName).GetComponent<SelectPlot>().NumOfBuildings == 2) return;
        if (Action == "FreeBuildNode")
        {
            BuildButton.SetActive(false);
            Build();
        }
        else
        {
            MainCam.SetActive(false);
            normalDice.SetActive(false);
            buildDice.SetActive(true);
            BuildPanel.SetActive(false);
            rollDiceEnviroment.SetActive(true);
        }
    }

    public void goToCredits()
    {        
        MenuButtons.SetActive(false);
        Credits.SetActive(true);
    }

    public void backToMainMenu()
    {        
        Credits.SetActive(false);
        MenuButtons.SetActive(true);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public bool isNotMoving()
    {
        return  started & move.walk.velocity == new Vector3(0, 0, 0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void wait()
    {
        rollResult.SetActive(false);
        return;
    }
}
