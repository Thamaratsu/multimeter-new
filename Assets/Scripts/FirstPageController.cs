using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPageController : MonoBehaviour
{
    [Header("Buttons")]
    public Button buttonResistor;
    public Button buttonVoltage;
    public Button buttonCurrent;

    void Start()
    {
        buttonResistor.onClick.AddListener(() => LoadScene("ResistorScene"));
        buttonVoltage.onClick.AddListener(() => LoadScene("VoltageScene"));
        buttonCurrent.onClick.AddListener(() => LoadScene("IR1"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
