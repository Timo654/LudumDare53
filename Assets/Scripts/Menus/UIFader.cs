using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UIFader
{
    //Create a class that actually inheritance from MonoBehaviour
    public class UIFaderMB : MonoBehaviour { }


    //Variable reference for the class
    private static UIFaderMB m_UIFaderMB;

    //Now Initialize the variable (instance)
    private static void Init()
    {
        //If the instance not exit the first time we call the static class
        if (m_UIFaderMB == null)
        {
            //Create an empty object called MyStatic
            GameObject gameObject = new("UIFader");


            //Add this script to the object
            m_UIFaderMB = gameObject.AddComponent<UIFaderMB>();
        }
    }



    //Now, a simple function
    public static void InitializeFader()
    {
        //Call the Initialization
        Init();
    }


    public static void FadeObjects(GameObject fadeInObject, CanvasGroup fadeInCanvasGroup, GameObject fadeOutObject, CanvasGroup fadeOutCanvasGroup) // alt is for extras
    {
        m_UIFaderMB.StartCoroutine(FadeBetweenObjects(fadeInObject, fadeInCanvasGroup, fadeOutObject, fadeOutCanvasGroup));
    }

    public static void FadeInImage(Image image)
    {
        m_UIFaderMB.StartCoroutine(ImageFadeIn(image));
    }

    public static void FadeOutImage(Image image)
    {
        m_UIFaderMB.StartCoroutine(ImageFadeOut(image));
    }

    public static void FadeCanvasGroup(CanvasGroup cg)
    {
        m_UIFaderMB.StartCoroutine(FadeInCanvas(cg));
    }

    static IEnumerator FadeBetweenObjects(GameObject fadeInObject, CanvasGroup fadeInCanvasGroup, GameObject fadeOutObject, CanvasGroup fadeOutCanvasGroup)
    {
        if (fadeOutCanvasGroup.alpha != 1f) yield break;
        fadeInCanvasGroup.alpha = 0f;
        fadeInCanvasGroup.interactable = true;
        fadeInObject.SetActive(true);
        fadeOutCanvasGroup.interactable = false;
        fadeOutObject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            fadeInCanvasGroup.alpha += 0.1f;
            fadeOutCanvasGroup.alpha -= 0.1f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        fadeOutObject.SetActive(false);
        if (!fadeInObject.activeSelf)
        {
            fadeInObject.SetActive(true); // failsafe/hack for the object just disappearing completely when mashing
        }
    }

    static IEnumerator ImageFadeIn(Image fadeInObject)
    {
        fadeInObject.color = new Color(fadeInObject.color.r, fadeInObject.color.g, fadeInObject.color.b, 0f);
        fadeInObject.transform.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            fadeInObject.color = new Color(1, 1, 1, i);
            yield return null;
        }
        fadeInObject.color = new Color(fadeInObject.color.r, fadeInObject.color.g, fadeInObject.color.b, 1f);
    }

    static IEnumerator ImageFadeOut(Image fadeInObject)
    {
        fadeInObject.color = new Color(fadeInObject.color.r, fadeInObject.color.g, fadeInObject.color.b, 1f);
        fadeInObject.transform.gameObject.SetActive(true);
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            fadeInObject.color = new Color(1, 1, 1, i);
            yield return null;
        }
        fadeInObject.color = new Color(fadeInObject.color.r, fadeInObject.color.g, fadeInObject.color.b, 0f);
    }

    static IEnumerator FadeInCanvas(CanvasGroup fadeInCanvasGroup)
    {
        fadeInCanvasGroup.alpha = 0f;
        fadeInCanvasGroup.transform.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            fadeInCanvasGroup.alpha += 0.1f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
}
