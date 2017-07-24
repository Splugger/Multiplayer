using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderAtResolution : MonoBehaviour
{
    public int resolution = 256;

    RenderTexture view;
    Transform viewDisplay;
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
        CreateView(resolution);
    }

    // Update is called once per frame
    void Update()
    {
        //this GetComponent call is important
        viewDisplay.GetComponent<RawImage>().texture = view;
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            CreateView(resolution);
        }
    }

    public void CreateView(int resolution)
    {
        viewDisplay = GameObject.Find("Canvas").transform.FindDeepChild("View");
        aspectRatio = screenWidth / screenHeight;

        view = new RenderTexture((int)(aspectRatio * resolution), resolution, 24, RenderTextureFormat.ARGB32);
        view.antiAliasing = 1;
        view.filterMode = FilterMode.Point;
        view.useMipMap = false;
        view.Create();

        camera.targetTexture = view;
    }
}
