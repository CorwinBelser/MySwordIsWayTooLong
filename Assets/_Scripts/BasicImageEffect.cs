using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicImageEffect : MonoBehaviour {
	public Material ImageEffectMaterial;

	void OnRenderImage(RenderTexture _in, RenderTexture _out) {
		Graphics.Blit(_in, _out, ImageEffectMaterial);
	}
}
