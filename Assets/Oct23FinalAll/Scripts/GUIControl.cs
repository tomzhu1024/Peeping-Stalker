using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControl : MonoBehaviour
{
    public float barValue = 0f;

    private GUIStyle borderStyle;
    private Texture2D borderTexture;
    private GUIStyle centerStyle;
    private Texture2D centerTexture;
    private GUIStyle cursorStyle;
    private Texture2D cursorTexture;
    private GUIStyle blockStyle;
    private Texture2D blockTexture;
    private Texture2D heartTexture;

    void Start()
    {
        borderStyle = new GUIStyle();
        borderTexture = new Texture2D(1, 1);
        borderTexture.SetPixel(0, 0, Color.white);
        borderTexture.Apply();
        borderStyle.normal.background = borderTexture;

        centerStyle = new GUIStyle();
        centerTexture = new Texture2D(1, 1);
        centerTexture.SetPixel(0, 0, new Color(0.08f, 0.95f, 0.19f));
        centerTexture.Apply();
        centerStyle.normal.background = centerTexture;

        cursorStyle = new GUIStyle();
        cursorTexture = new Texture2D(1, 1);
        cursorTexture.SetPixel(0, 0, new Color(0.66f, 0f, 0.59f));
        cursorTexture.Apply();
        cursorStyle.normal.background = cursorTexture;

        heartTexture = Resources.Load("heart") as Texture2D;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        DrawDistance(Screen.width / 2 - 400, 100, 800, 50, 5, 5, 0.45f, 0.55f, 15, 80, 0.5f);
        DrawLife(Screen.width / 2 + 425, 100, 50, 50, 25, 3);
    }

    void DrawBorder(float x, float y, float width, float height, float thickness)
    {
        GUI.Box(new Rect(x, y, width, thickness), GUIContent.none, borderStyle);
        GUI.Box(new Rect(x, y, thickness, height), GUIContent.none, borderStyle);
        GUI.Box(new Rect(x, y + height - thickness, width, thickness), GUIContent.none, borderStyle);
        GUI.Box(new Rect(x + width - thickness, y, thickness, height), GUIContent.none, borderStyle);
    }

    float GetWeightedMidPoint(float start, float end, float percentage)
    {
        return start + (end - start) * percentage;
    }

    void DrawDistance(float x, float y, float width, float height, float borderThickness, float borderPadding,
        float centerStartPercentage, float centerEndPercentage, float cursorWidth, float cursorHeight, float cursorPercentage)
    {
        float startX = x + borderThickness + borderPadding;
        float endX = x + width - borderThickness - borderPadding;
        float centerStartX = GetWeightedMidPoint(startX, endX, centerStartPercentage);
        float centerEndX = GetWeightedMidPoint(startX, endX, centerEndPercentage);
        float cursorX = GetWeightedMidPoint(startX, endX, cursorPercentage);

        DrawBorder(x, y, width, height, borderThickness);
        // Draw center
        GUI.Box(
            new Rect(centerStartX, y + borderThickness + borderPadding, centerEndX - centerStartX,
                height - 2 * (borderThickness + borderPadding)), GUIContent.none, centerStyle);
        // Draw cursor
        GUI.Box(new Rect(cursorX - cursorWidth / 2, y + height / 2 - cursorHeight / 2, cursorWidth, cursorHeight), GUIContent.none,
            cursorStyle);
    }

    void DrawLife(float x, float y, float width, float height, float padding, int value)
    {
        for (int i = 0; i < value; i++)
        {
            GUI.DrawTexture(new Rect(x + i * (width + padding), y, width, height), heartTexture);
        }
    }
}
