using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode] 
public class ImageEffect : MonoBehaviour {

    public Material mat;

    void OnRenderImage( RenderTexture src, RenderTexture dest )
    {//src is our rendered image from our camera "source:
     // we will intercept this Image and modify it
     //then send it to our screen "Destination" dest

        Graphics.Blit(src, dest, mat);        

    }
}
