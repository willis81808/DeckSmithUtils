using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WebCardArt : MonoBehaviour
{
    internal DeckSmithUtil.TextureFuture TextureFuture { get; set; }

    private RawImage renderer;

    void Start()
    {
        renderer = gameObject.AddComponent<RawImage>();

        TextureFuture.OnComplete += SetTexture;
        TextureFuture.Ready = true;
    }

    private void SetTexture(Texture2D texture)
    {
        //renderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one / 2f, 100f);
        renderer.texture = texture;
    }

    void Update()
    {
        if (GetComponent<RectTransform>() is RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = Vector2.one / 2f;
            rt.sizeDelta = Vector2.zero;
            Debug.Log("Found RectTransform");
        }
        else
        {
            Debug.Log("Didn't find RectTransform :(");
        }
    }
}