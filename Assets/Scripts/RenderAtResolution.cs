using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderAtResolution : MonoBehaviour
{
    public int resolution = 256;

    RenderTexture view;
    GameObject viewDisplay;
    RawImage image;
    Camera camera;
    float aspectRatio;

    float screenWidth;
    float screenHeight;

    // Use this for initialization
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        camera = GetComponent<Camera>();

        //create viewDisplay
        GameObject canvasObj = new GameObject();
        canvasObj.name = "Canvas";
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        canvasTransform.anchorMin = Vector2.zero;
        canvasTransform.anchorMax = Vector2.one;
        canvasTransform.position = Vector2.zero;
        canvasTransform.sizeDelta = Vector2.zero;

        viewDisplay = new GameObject();
        viewDisplay.name = "View";
        viewDisplay.transform.parent = canvasObj.transform;
        image = viewDisplay.AddComponent<RawImage>();
        RectTransform imageTransform = image.GetComponent<RectTransform>();
        imageTransform.anchorMin = Vector2.zero;
        imageTransform.anchorMax = Vector2.one;
        imageTransform.position = Vector2.zero;
        imageTransform.sizeDelta = Vector2.zero;

        CreateView(resolution);
    }

    // Update is called once per frame
    void Update()
    {
        //this GetComponent call is important
        image.texture = view;
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            CreateView(resolution);
        }
    }

    public void CreateView(int resolution)
    {
        aspectRatio = screenWidth / screenHeight;

        view = new RenderTexture((int)(aspectRatio * resolution), resolution, 24, RenderTextureFormat.ARGB32);
        view.antiAliasing = 1;
        view.filterMode = FilterMode.Point;
        view.useMipMap = false;
        view.Create();

        camera.targetTexture = view;
    }
}
