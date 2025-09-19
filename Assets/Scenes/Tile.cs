using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material defualtMat;
    private Color defualtColor;
    
    public void OnInitialized(Color clr)
    {
        defualtColor = clr;
        meshRenderer.material.color = defualtColor;
    }

    public void HighLight(Color clr)
    {
        meshRenderer.material.color = clr;
    }

    public void DeHighlight()
    {
        meshRenderer.material.color = defualtColor;
    }
}
