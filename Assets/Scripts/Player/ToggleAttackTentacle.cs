using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAttackTentacle : MonoBehaviour
{
    private AttackTentacle attackTentacleScript;
    private LineRenderer lineRenderer;

    private int originalSegmentLength;
    public float transitionDuration = 0.5f;

    private bool transitioning = false;
    private float transitionTimer = 0.0f;

    private bool pendingToggle = false;

    void Start()
    {
        attackTentacleScript = GetComponent<AttackTentacle>();
        lineRenderer = GetComponent<LineRenderer>();

        originalSegmentLength = attackTentacleScript.segmentLength;
    }

    void Update()
    {
        if (transitioning)
        {
            transitionTimer += Time.deltaTime;

            float t = Mathf.Clamp01(transitionTimer / transitionDuration);
            attackTentacleScript.segmentLength = (int)Mathf.Lerp(originalSegmentLength, 0, t);

            if (t >= 1.0f)
            {
                transitioning = false;

                if (pendingToggle)
                {
                    pendingToggle = false;
                    ToggleScriptAndRenderer();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!attackTentacleScript.enabled)
            {
                attackTentacleScript.segmentLength = originalSegmentLength;
                ToggleScriptAndRenderer();
            }
            else if (!transitioning)
            {
                transitioning = true;
                transitionTimer = 0.0f;
                pendingToggle = true;
            }
        }
    }

    void ToggleScriptAndRenderer()
    {
        attackTentacleScript.enabled = !attackTentacleScript.enabled;

        lineRenderer.enabled = !lineRenderer.enabled;
    }
}