using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSide : MonoBehaviour
{
    public GameObject diceObj;
    public GameObject BuildDiceObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        diceObj.GetComponent<Dice>().side = other.name;
        BuildDiceObj.GetComponent<BuildDice>().side = other.name;
    }
}
