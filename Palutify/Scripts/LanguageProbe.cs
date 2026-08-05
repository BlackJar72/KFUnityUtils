using UnityEngine;

public class LanguageProbe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color colorA = Color.cyan;
        Color colorB = Color.cyan;
        Color colorC = new Color(0.2f, 0.3f, 0.4f);
        Color colorD = new Color(0.2f, 0.3f, 0.4f);
        Color colorE = new Color(0.4f, 0.3f, 0.2f);

        Debug.Log("ColorA == ColorB? " + (colorA == colorB));
        Debug.Log("ColorC == ColorD? " + (colorC == colorD));
        Debug.Log("ColorC == ColorE? " + (colorC == colorE));        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
