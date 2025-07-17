using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CharacterController : MonoBehaviour
{
    private ICharacter character;

    private void Awake()
    {
        if (!TryGetComponent<ICharacter>(out character))
        {
            Debug.Break();
        }
    }

    public void OnAttackPerformed()
    {
        character.UseAttack(true);
    }

    public void OnAttackCanceled()
    {
        character.UseAttack(false);
    }

    public void OnBlockPerformed()
    {
        character.UseBlock(true);
    }

    public void OnBlockCanceled()
    {
        character.UseBlock(false);
    }

    public void OnDodgePerformed()
    {
        character.UseDodge();
    }

    public void OnSpecialPerformed()
    {
        character.UseSpecial();
    }

}
