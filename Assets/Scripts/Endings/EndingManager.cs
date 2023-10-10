using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSecondsRealtime(5f);
        LevelChangerScript._instance.FadeToLevel("Credits");
    }
}
