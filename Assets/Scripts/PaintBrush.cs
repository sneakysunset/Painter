using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public float paintingDistance = 2;
    public LayerMask paintingLayer;
    public Transform paintingPoint;
    public float brushWidth = 0.05f;
    public Texture2D brushTex;
    bool hitPaintArea;

    void Update()
    {
        PaintDetection();
    }

    void PaintDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(paintingPoint.position, paintingPoint.forward, out hit, paintingDistance, paintingLayer))
        {
            PaintArea paintArea;
            if (!hit.collider.TryGetComponent<PaintArea>(out paintArea)) return;
            paintArea.Paint(hit.textureCoord, brushWidth, brushTex);
            hitPaintArea = true;
        }
        else hitPaintArea = false;
    }

    private void OnDrawGizmos()
    {
        if(hitPaintArea) Gizmos.color = Color.green;
        else Gizmos.color = Color.blue;
        Gizmos.DrawRay(paintingPoint.position, paintingPoint.forward * paintingDistance);
    }
}

