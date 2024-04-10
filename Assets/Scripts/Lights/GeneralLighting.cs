using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GeneralLighting : MonoBehaviour
{
    public Light2D light2D;
    public Color color1 = Color.white;
    public Color color2 = Color.red;
    public float color1Duration = 3f;
    public float color2Duration = 3f;
    public float transitionDuration = 1f; // Duration for color transition

    private float timer;
    private bool transitioning = false;
    private Color startColor;
    private Color endColor;

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }

        // Start with color1
        light2D.color = color1;
        timer = color1Duration;
        startColor = color1;
        endColor = color2;
    }

    private void Update()
    {
        if (!transitioning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                transitioning = true;
                timer = transitionDuration;
                startColor = light2D.color;
                endColor = (startColor == color1) ? color2 : color1;
            }
        }
        else
        {
            timer -= Time.deltaTime;

            float t = 1f - Mathf.Clamp01(timer / transitionDuration);
            light2D.color = Color.Lerp(startColor, endColor, t);

            if (timer <= 0f)
            {
                transitioning = false;
                timer = (endColor == color1) ? color1Duration : color2Duration;
            }
        }
    }
}
