using UnityEngine;

public class PreloadConfig : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (BuildConstants.isMobile) Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
}
