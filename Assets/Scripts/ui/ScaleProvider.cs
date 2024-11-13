using System;
using UnityEngine;
using UnityEngine.UI;

public class ScaleProvider : MonoBehaviour {
	
	private RectTransform rt;
	private CanvasScaler canvasScaler;
	private Canvas canvas;

	private void Awake() {
		rt = GetComponent<RectTransform>();
		canvasScaler = GetComponentInParent<CanvasScaler>();
		canvas = GetComponentInParent<Canvas>();
	}      

	public float getScale() {
		if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
			return rt.lossyScale.y;	
		}
		else if (canvas.renderMode == RenderMode.ScreenSpaceCamera) {
			return Screen.height / canvasScaler.referenceResolution.y;
		}
		else {
			throw new Exception("Can not get scale when in a world space canvas");
		}
	}
}
