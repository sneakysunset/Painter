using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PaintArea : MonoBehaviour
{
    public int textureResolution = 256;
    public RenderTexture paintRT;
    Material mat;
    private void OnEnable()
    {
        mat = GetComponent<Renderer>().material;
        paintRT = RenderTexture.GetTemporary(textureResolution, textureResolution, 32, RenderTextureFormat.Default);
        mat.SetTexture("_PaintTex", paintRT);
        ClearOutRenderTexture(paintRT);
    }

    private void OnDisable()
    {
        RenderTexture.ReleaseTemporary(paintRT);
    }

    public void Paint(Vector2 uv, float brushWidth, Texture2D brushTex)
    {
        //Activate Render Texture
        RenderTexture.active = paintRT;
        //SaveMatrix
        GL.PushMatrix();
        //Set Up Matrix for correct size
        GL.LoadPixelMatrix(0, textureResolution, textureResolution, 0);

        //Resize Uvs and width for colorResolutions
        uv.x *= textureResolution; 
        uv.y = textureResolution * (1 - uv.y);
        brushWidth *= textureResolution;

        //Recenter painting point on UV (from top right to middle by moving half the uvs size on x and y)
        Rect paintRect = new Rect(uv.x - brushWidth * .5f, uv.y - brushWidth * .5f, brushWidth, brushWidth);
        Graphics.DrawTexture(paintRect, brushTex, new Rect(0, 0, 1, 1), 0, 0, 0, 0, Color.white, null);
        GL.PopMatrix();

        RenderTexture.active = null; 
    }

    void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        RenderTexture.active = null;
    }
    private void Update()
    {
        mat.SetTexture("_PaintTex", paintRT);

    }
}
