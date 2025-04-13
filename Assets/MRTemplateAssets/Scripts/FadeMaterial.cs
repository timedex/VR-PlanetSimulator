using System.Collections;
using UnityEngine;

public class FadeMaterial : MonoBehaviour
{
    // attached game object for fading
    public GameObject Environment;

    // fade speed length
    public float fadeSpeed;

    Coroutine m_FadeCoroutine;

    public void FadeSkybox(bool visible)
    {
        if (m_FadeCoroutine != null)
            StopCoroutine(m_FadeCoroutine);

        m_FadeCoroutine = StartCoroutine(FadeURP(visible));
    }

    //Fade Coroutine
    public IEnumerator FadeURP(bool visible)
    {
        Renderer rend = Environment.transform.GetComponent<Renderer>();
        float alphaValue = rend.material.GetFloat("_Alpha");

        if (visible)
        {
            //while loop to deincrement Alpha value until object is invisible
            while (rend.material.GetFloat("_Alpha") > 0f)
            {
                alphaValue -= Time.deltaTime / fadeSpeed;
                rend.material.SetFloat("_Alpha", alphaValue);
                yield return null;
            }
            rend.material.SetFloat("_Alpha", 0f);
        }
        else if (!visible)
        {
            //while loop to increment object Alpha value until object is opaque
            while (rend.material.GetFloat("_Alpha") < 1f)
            {
                alphaValue += Time.deltaTime / fadeSpeed;
                rend.material.SetFloat("_Alpha", alphaValue);
                yield return null;
            }
            rend.material.SetFloat("_Alpha", 1f);
        }
    }
    
    public IEnumerator FadeBuiltIn(bool visible)
    {
        print("Fade BuiltIn visible: " + visible);;
        Renderer rend = Environment.transform.GetComponent<Renderer>();
        Material mat = rend.material;
    
        // Asegúrate de que el shader está en modo fade/transparente
        mat.SetFloat("_Mode", 2); // 2 = Fade
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        Color color = mat.color;
        float alpha = color.a;

        if (visible)
        {
            while (alpha > 0f)
            {
                alpha -= Time.deltaTime / fadeSpeed;
                alpha = Mathf.Clamp01(alpha);
                color.a = alpha;
                mat.color = color;
                yield return null;
            }
        }
        else
        {
            while (alpha < 1f)
            {
                alpha += Time.deltaTime / fadeSpeed;
                alpha = Mathf.Clamp01(alpha);
                color.a = alpha;
                mat.color = color;
                yield return null;
            }
        }
    }

}
