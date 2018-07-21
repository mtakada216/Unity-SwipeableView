using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TextureDownloader : MonoBehaviour
{
    private static TextureDownloader i;
    public static TextureDownloader I
    {
        get
        {
            if (i == null)
            {
                i = FindObjectOfType(typeof(TextureDownloader)) as TextureDownloader;
            }
            return i;
        }
    }

    public void Load(string url, System.Action<Texture> onSuccess)
    {
        StartCoroutine(LoadTextureAsync(url, onSuccess));
    }

    private static IEnumerator LoadTextureAsync(string url, System.Action<Texture> onSuccess)
    {
        using(var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.LogErrorFormat("Error: {0}", www.error);
                yield break;
            }

            onSuccess.Invoke(((DownloadHandlerTexture) www.downloadHandler).texture);
        }
    }
}