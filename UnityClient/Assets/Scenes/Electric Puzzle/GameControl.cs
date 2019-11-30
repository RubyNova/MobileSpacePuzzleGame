using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    private Transform[] pieces;

    [SerializeField]
    private GameObject winText;

    public static bool youWin;

    private void Start()
    {
        winText.SetActive(false);
        youWin = false;
    }

    private void Update()
    {
        if(
           pieces[0].rotation.z == 0 &&

           pieces[1].rotation.z == -180  ||
           pieces[1].rotation.z == 0 &&


           pieces[2].rotation.z == 180 ||
           pieces[2].rotation.z == 0 &&


           pieces[3].rotation.z == -180 ||
           pieces[3].rotation.z == 0 &&

          
           pieces[5].rotation.z == -180 ||
           pieces[5].rotation.z == 0 &&


           pieces[6].rotation.z == 0 &&

           pieces[7].rotation.z == -180 ||
           pieces[7].rotation.z == 0 &&
           pieces[9].rotation.z == 0 &&

           
           pieces[10].rotation.z == -180 ||
           pieces[10].rotation.z == 0 &&

           pieces[11].rotation.z == 180 ||
           pieces[11].rotation.z == 0 &&

           pieces[12].rotation.z == -180 ||
           pieces[12].rotation.z == 0 &&

           pieces[13].rotation.z == 180 ||
           pieces[13].rotation.z == 0 &&


           pieces[14].rotation.z == 0 &&


           pieces[15].rotation.z == 180 ||
           pieces[15].rotation.z == 0 
           )
        {
            youWin = true;
            winText.SetActive(true);
        }
    }

}
