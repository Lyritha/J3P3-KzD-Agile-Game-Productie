using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// this script is added so i dont have to manually change the color from all of the child objects pls keep this thing
/// </summary>
public class ChangeColorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject thisGameObject = gameObject;
        int childCount = thisGameObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            //Unshits the player
            if (i < (childCount - 2))
            {
                //colors the ship brown
                thisGameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color32(122, 65, 30, 255);
            }
        }
    }
}
