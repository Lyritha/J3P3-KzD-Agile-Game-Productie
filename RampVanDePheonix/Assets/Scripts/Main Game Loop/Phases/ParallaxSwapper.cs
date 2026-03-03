using UnityEngine;
using UnityEngine.UI;

public class ParallaxSwapper : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Image background;
    [SerializeField] Image foreground;

    [Header("AchterhoekMaterials")]
    [SerializeField] Material achterhoekBackground;
    [SerializeField] Material achterhoekForeground;

    [Header("PheonixMaterials")]
    [SerializeField] Material pheonixBackground;
    [SerializeField] Material pheonixForeground;

    [Header("amerikaMaterials")]
    [SerializeField] Material amerikaBackground;
    [SerializeField] Material amerikaForeground;

    public void SetGrounds(Fases fase)
    {
        switch (fase)
        {
            case Fases.Achterhoek:
                background.material = achterhoekBackground;
                foreground.material = achterhoekForeground;
                break;
            case Fases.Pheonix:
                background.material = pheonixBackground;
                foreground.material = pheonixForeground;
                break;
            case Fases.Amerika:
                background.material = amerikaBackground;
                foreground.material = amerikaForeground;
                break;
        }
    }
}
