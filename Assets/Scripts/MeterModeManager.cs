using UnityEngine;

// Enum สำหรับโหมดของมัลติมิเตอร์
public enum MeterMode
{
    OFF,        // ปิดเครื่อง

    // AC Voltage
    ACV1000,
    ACV250,
    ACV50,
    ACV10,

    // Resistance (Ohm)
    OHM10K,
    OHM1K,
    OHM100,
    OHM10,
    OHM1,

    // DC Current
    DCMAX250,
    DCMAX25,
    DCMAX2_5,
    DC100uA,    // รวมกับ DCV0_25

    // DC Voltage
    DCV0_25,
    DCV0_5,
    DCV2_5,
    DCV10,
    DCV50,
    DCV250,
    DCV100
}
