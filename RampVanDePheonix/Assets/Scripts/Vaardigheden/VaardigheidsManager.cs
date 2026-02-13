using UnityEngine;

public enum VaardigType
{
    Leer,
    Kapitaal,
    Aanpas,
    Bouw,
    Sociaal
}
public class VaardigheidsManager : MonoBehaviour
{
    [SerializeField] int leerVermogen = 0;
    [SerializeField] int kapitaal = 0;
    [SerializeField] int aanpassingsVermogen = 0;
    [SerializeField] int bouwKunde = 0;
    [SerializeField] int socialiteit = 0;

    void Start()
    {
        leerVermogen = 0;
        kapitaal = 0;
        aanpassingsVermogen = 0;
        bouwKunde = 0;
        socialiteit = 0;
    }

    public void AddVaardigheden(VaardigType type, int amount)
    {
        switch (type)
        {
            case VaardigType.Leer:
                leerVermogen += amount;
                break;
            case VaardigType.Kapitaal:
                kapitaal += amount;
                break;
            case VaardigType.Aanpas:
                aanpassingsVermogen += amount;
                break;
            case VaardigType.Bouw:
                bouwKunde += amount;
                break;
            case VaardigType.Sociaal:
                socialiteit += amount;
                break;
        }
    }
}
