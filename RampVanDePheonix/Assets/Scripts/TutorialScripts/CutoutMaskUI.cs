using UnityEngine;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material mat = new Material(base.materialForRendering);
            mat.SetInt("_StencilComp", (int)UnityEngine.Rendering.CompareFunction.NotEqual);
            return mat;
        }
    }
}
