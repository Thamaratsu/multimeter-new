using UnityEngine;

public class Wire : MonoBehaviour
{
    [Header("Wire Connection")]
    public Terminal startTerminal;
    public Terminal endTerminal;

    [Header("Wire Electrical Properties")]
    public float resistance = 0.1f; // ความต้านทานของสาย
    public float voltage; 
    public float current; 

    [Header("Probe Settings")]
    public float probeThresholdX = 0.2f;
    public float probeThresholdY = 0.5f;

    void Update()
    {
        if (startTerminal != null && endTerminal != null)
        {
            // Voltage = ความต่างแรงดันระหว่าง Terminal
            voltage = Mathf.Abs((startTerminal.node?.voltage ?? 0f) - (endTerminal.node?.voltage ?? 0f));

            // Current = กระแสจาก Terminal อีกด้าน
            current = startTerminal.current;

            // ส่งต่อกระแสไปยัง Terminal อีกด้าน
            endTerminal.current = current;
        }
        else
        {
            voltage = 0f;
            current = 0f;
        }
    }

    public bool IsProbeTouchingY(Transform probe)
    {
        bool isTouchingStart = startTerminal != null && 
            Mathf.Abs(probe.position.x - startTerminal.transform.position.x) <= probeThresholdX &&
            Mathf.Abs(probe.position.y - startTerminal.transform.position.y) <= probeThresholdY;

        bool isTouchingEnd = endTerminal != null &&
            Mathf.Abs(probe.position.x - endTerminal.transform.position.x) <= probeThresholdX &&
            Mathf.Abs(probe.position.y - endTerminal.transform.position.y) <= probeThresholdY;

        return isTouchingStart || isTouchingEnd;
    }

    // API
    public float GetVoltage() => voltage;
    public float GetCurrent() => current;
    public float GetResistance() => resistance;
}
