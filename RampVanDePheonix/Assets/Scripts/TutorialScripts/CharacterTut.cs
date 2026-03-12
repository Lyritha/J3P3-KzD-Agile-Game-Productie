using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTut : Character
{
    protected override void ToggleSelected(bool forceState = false, bool forcedState = false)
    {
        parent.SetSelectedCharacter(this);
    }
}
