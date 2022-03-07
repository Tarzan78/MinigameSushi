using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject NinjaGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //StartNinjaGame();
    }

    private void StartNinjaGame()
    {
        NinjaGame.GetComponent<NinjaGameManager>().StratNinjaGame();
    } 
}
