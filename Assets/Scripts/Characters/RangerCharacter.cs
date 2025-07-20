using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerCharacter : MonoBehaviour, ICharacter
{


    public void UseAttack(bool attackStatus)
    {
        //if (attackStatus)
        //    Debug.Log("Ranger uses attack!");
        //else
        //    Debug.Log("Ranger stops using attack!");

        // preform some attack logic for Ranger
    }
    public void UseBlock(bool blockStatus)
    {
        //if (blockStatus)
        //    Debug.Log("Ranger uses block!");
        //else
        //    Debug.Log("Ranger stops using block!");
    }

    public void UseDodge()
    {
        //Debug.Log("Ranger uses dodge!");
    }

    public void UseSpecial()
    {
        //Debug.Log("Ranger uses special!");
    }
}
