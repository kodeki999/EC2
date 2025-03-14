using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class underWater : BasePP
{

    [Range(0.0f, 1.0f)]
    public float height = 0.3f;
    [Range(0.0f, 100.0f)]
    public float softenEdge;
    [Range(0.0f, 1.0f)]
    public float shade;
    [Range(0.0f, 1.0f)]
    public float tintStrength;

    
    float inter_fact = 0.5f;

    public Color tintColor = Color.white;

    private void OnValidate()
    {
        if (!init) Init();

        SetProperties();


    }

    protected void SetProperties()
    {
        float tintHeight = height * texSize.y;
        shader.SetFloat("tintHeight", tintHeight);
        shader.SetFloat("edgeWidth", tintHeight * softenEdge / 100.0f);
        shader.SetFloat("shade", shade);
        shader.SetFloat("tintStrength", tintStrength);
        shader.SetVector("tintColor", tintColor);
        shader.SetFloat("inter_fact", inter_fact);
    }

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogError("Compute Shader non supportato sul visore!");
        }

        if (!init || shader == null || transform.position.y > -2)
        {
            Graphics.Blit(source, destination);
            

        }
        else
        {
            CheckResolution(out _);
            DispatchWithSource(ref source, ref destination);
        }
    }

}
