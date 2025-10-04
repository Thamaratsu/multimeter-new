using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [Header("Buttons")]
    public Button buttonResistor;
    public Button buttonVoltage;
    public Button buttonCurrent;

    void Start()
    {
        buttonResistor.onClick.AddListener(() => LoadScene("FirstPage"));
        buttonVoltage.onClick.AddListener(() => LoadScene("VoltageScene"));
        buttonCurrent.onClick.AddListener(() => LoadScene("CurrentScene"));
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}