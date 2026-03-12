using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private float force;
    public Vector3 velocity;
    public Vector3 angularV;
    public GameObject diceCamera;
    public string side;
    public GameObject GameMaster;
    public GameLogic GameLogic;
    public bool landed;
    public int started;

    // Start is called before the first frame update
    void Start()
    {
        GameLogic = GameMaster.GetComponent<GameLogic>();
        gameObject.transform.position = new Vector3(865, 24, 1950);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.right * force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0, force, 0));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(force, 0, 0));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, force));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
    }

    private void OnEnable()
    {
        landed = false;
        started = 0;
        gameObject.transform.position = new Vector3(865, 24, 1950);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        diceCamera.transform.position = new Vector3(848.73999f, 44.0999985f, 1949.87f);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.right * force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -force);
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0, force, 0));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(force, 0, 0));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
        gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, force));
        force = ((float)Random.Range(1f, 6f)) * 1200f;
    }
    // Update is called once per frame
    void Update()
    {
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        angularV = gameObject.GetComponent<Rigidbody>().angularVelocity;
        if (velocity == new Vector3(0,0,0) & angularV == new Vector3(0, 0, 0) & !landed & started > 30)
        {
            landed = true;
            diceCamera.transform.position = new Vector3(transform.position.x - 3, transform.position.y + 3, transform.position.z);
            Invoke("wait" , 2f);    
        }
        started ++;  // begin checking after a few frames so that the dice has started moving;
    }

    public void wait()
    {
        GameLogic.move.Roll = int.Parse(side.Substring(4));
        GameLogic.move.MyTurn = true;
        gameObject.transform.parent.gameObject.SetActive(false);
        GameLogic.SwapCamToPlayer();
    }
}
