using UnityEngine.UI;
using UnityEngine;

class TextInputDetector : MonoBehaviour
{
    public InputField inp;
    public GameObject AdminPanel;
    public GameObject Panel;
    public Button Button;

    void Start()
    {
        Button btn = Button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (inp.text == "admin" | inp.text == "Admin" | inp.text == "ADMIN")
        {
            //Debug.Log("Admin entered as input");
            AdminPanel.SetActive(true);   //Load admin console panel
        }
        else
        {
            Panel.SetActive(true); //Loads next page w/o admin
        }
    }
}
