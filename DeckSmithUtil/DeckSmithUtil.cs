using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeckSmithUtil : MonoBehaviour
{
    internal static Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();

    private static DeckSmithUtil _instance;
    public static DeckSmithUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("DeckSmith Singleton");
                _instance = go.AddComponent<DeckSmithUtil>();
            }
            return _instance;
        }
    }

    public class TextureFuture
    {
        public delegate void OnCompleteDelegate(Texture2D texture);
        public event OnCompleteDelegate OnComplete;

        public bool Ready { get; set; }

        internal void LoadTexture(Texture2D texture)
        {
            OnComplete?.Invoke(texture);
        }
    }

    public GameObject GetArtFromUrl(string url)
    {
        var future = new TextureFuture();

        var go = new GameObject("Web Card Art");
        go.AddComponent<WebCardArt>().TextureFuture = future;

        StartCoroutine(GetTexture(url, future));

        return go;
    }

    internal IEnumerator GetTexture(string url, TextureFuture future)
    {
        yield return new WaitUntil(() => future.Ready);

        if (cachedTextures.TryGetValue(url, out var t))
        {
            future.LoadTexture(t);
            yield break;
        }
        else
        {
            using (var uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                    yield break;
                }

                var texture = DownloadHandlerTexture.GetContent(uwr);
                future.LoadTexture(texture);
            }
        }
    }
}
