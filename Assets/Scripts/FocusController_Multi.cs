using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FocusTargetData
{
    public Transform targetTransform;
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;
    [HideInInspector] public Vector3 originalScale;
    [HideInInspector] public Transform originalParent;
}
public class FocusController_Multi : MonoBehaviour
{
    [Header("View Panels")]
    public GameObject contextPanel;
    public GameObject focusPanel;

    [Header("Objects to Focus")]
    public List<FocusTargetData> focusTargets = new List<FocusTargetData>();

    [Header("Settings")]
    public float zoomScaleFactor = 4f;
    public Vector3 focusPosition = Vector3.zero; // ตำแหน่งกลาง Focus Panel

    void Start()
    {
        // ตรวจสอบให้แน่ใจว่า Focus Panel ถูกซ่อนไว้ตั้งแต่แรก
        if (focusPanel != null)
        {
            focusPanel.SetActive(false);
        }
    }

    // --- ฟังก์ชัน 1: กดปุ่ม Focus ---
    public void FocusObjects()
    {
        if (contextPanel != null) contextPanel.SetActive(false);

        foreach (var data in focusTargets)
        {
            if (data.targetTransform == null) continue;

            // 1. เก็บค่าเดิมทั้งหมดก่อน
            data.originalParent = data.targetTransform.parent;
            data.originalPosition = data.targetTransform.localPosition;
            data.originalRotation = data.targetTransform.localRotation;
            data.originalScale = data.targetTransform.localScale;

            // 2. ย้าย Object ไปอยู่ใน Focus Panel 
            // 'worldPositionStays: true' จะรักษาตำแหน่งในโลก 3 มิติไว้ขณะเปลี่ยน Parent
            data.targetTransform.SetParent(focusPanel.transform, worldPositionStays: true);

            // 3. ขยายขนาดและจัดตำแหน่งใหม่ใน Focus View
            data.targetTransform.localPosition = focusPosition;
            data.targetTransform.localRotation = Quaternion.identity;
            data.targetTransform.localScale = data.originalScale * zoomScaleFactor;
        }

        if (focusPanel != null) focusPanel.SetActive(true);
    }

    // --- ฟังก์ชัน 2: กดปุ่มกลับ ---
    public void ReturnToContext()
    {
        if (focusPanel != null) focusPanel.SetActive(false);

        foreach (var data in focusTargets)
        {
            if (data.targetTransform == null) continue;

            // 1. นำ Object กลับไปสู่ Parent เดิม
            data.targetTransform.SetParent(data.originalParent, worldPositionStays: true);

            // 2. คืนค่าตำแหน่ง, การหมุน, และขนาดเดิม
            data.targetTransform.localPosition = data.originalPosition;
            data.targetTransform.localRotation = data.originalRotation;
            data.targetTransform.localScale = data.originalScale;
        }

        if (contextPanel != null) contextPanel.SetActive(true);
    }
}