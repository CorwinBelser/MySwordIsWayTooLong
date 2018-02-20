using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspirationalQuoteDisplayer : MonoBehaviour {

    public Canvas CANVAS; /* Reference to the canvas */
    public GameObject TEXT_PREFAB; /* Reference to Text component of the canvas to display messages */
    public float TEXT_DISPLAY_TIME = 5f; /* How long should a quote appear on screen */

    public string[] INSPIRATIONAL_QUOTES;
    private int index;

	// Use this for initialization
	void Start () {
        if (INSPIRATIONAL_QUOTES.Length == 0)
            this.enabled = false;
        else
            index = 0;
	}

    public void DisplayQuote()
    {
        StartCoroutine(InstillInspiration());
    }

    private IEnumerator InstillInspiration()
    {
        GameObject textUI = Instantiate(TEXT_PREFAB, CANVAS.transform);
        Text quote = textUI.GetComponent<Text>();
        quote.text = INSPIRATIONAL_QUOTES[index];
        quote.color = Random.ColorHSV();
        index = (index + 1) % INSPIRATIONAL_QUOTES.Length;

        /* Animate the text rotating and scaling */
        float secondsPassed = 0;
        float direction = Random.Range(-2f, 2f);
        RectTransform rect = textUI.GetComponent<RectTransform>();
        while (secondsPassed < TEXT_DISPLAY_TIME)
        {
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x + Mathf.PerlinNoise(Time.time, secondsPassed) * direction,
                rect.anchoredPosition.y + Mathf.PerlinNoise(Time.time, secondsPassed) * direction);
            textUI.transform.Rotate(Vector3.forward, direction);
            rect.localScale = new Vector3(
                rect.localScale.x + Mathf.PerlinNoise(Time.time, secondsPassed) * direction * Time.deltaTime,
                rect.localScale.y + Mathf.PerlinNoise(Time.time, secondsPassed) * direction * Time.deltaTime,
                rect.localScale.z);
            secondsPassed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(textUI);
    }

}
