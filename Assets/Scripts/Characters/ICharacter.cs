using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{    
    public void UseAttack(bool attackStatus); // main attack, usually lmb; status is used to determine wether attack is held or not 

    public void UseBlock(bool blockStatus); // blocking/shielding of some kind, usually rmb; status is used to determine wether block is held or not

    public void UseDodge(); // movement skill or utility of some kind

    public void UseSpecial(); // craftable special skill
}
