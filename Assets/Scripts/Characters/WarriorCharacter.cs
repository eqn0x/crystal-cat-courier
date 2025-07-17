using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacter : MonoBehaviour, ICharacter
{
    public void UseAttack(bool attackStatus)
    {
        if (attackStatus)
            Debug.Log("Warrior uses attack!");
        else
            Debug.Log("Warrior stops using attack!");

        // preform some attack logic for warrior
    }
    public void UseBlock(bool blockStatus)
    {
        if (blockStatus)
            Debug.Log("Warrior uses block!");
        else
            Debug.Log("Warrior stops using block!");
    }

    public void UseDodge()
    {
        Debug.Log("Warrior uses dodge!");
    }

    public void UseSpecial()
    {
        Debug.Log("Warrior uses special!");
    }
}
