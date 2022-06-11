using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            DestroyImmediate(this.gameObject);
    }
}
