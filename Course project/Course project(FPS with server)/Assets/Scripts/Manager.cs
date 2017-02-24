using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public GameObject NamePrefab;

    public GameObject NicknamePanel;
    public GameObject ConnectionPanel;
    public GameObject ConReadyText;
    public Text ErrorNickname;
    public InputField NicknameInputField;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        ConnectionPanel.SetActive(true);
        ServerConnection.Instance.MyPlayer.SetActive(false);
    }

    private void Update()
    {
        if (ServerConnection.Instance.IsConnect)
        {
            ServerConnection.Instance.IsConnect = false;
            ConReadyText.SetActive(true);
            NicknamePanel.SetActive(true);
            ConnectionPanel.SetActive(false);
        }
    }
    
    public void Play()
    {
        try
        {
            if (NicknameInputField.text.Length != 0)
            {
                ServerConnection.Name = NicknameInputField.text;
                ServerConnection.Instance.SendMessage(NicknameInputField.text);

                ErrorNickname.gameObject.SetActive(false);

                ServerConnection.Instance.MyPlayer.SetActive(true);
                ServerConnection.Instance.MyPlayer.GetComponent<MouseLook>().enabled = true;
                ServerConnection.Instance.MyPlayer.GetComponent<PlayerMovement>().enabled = true;
                ServerConnection.Instance.SendMessage(10, 10, 10);
                ServerConnection.Instance.SendMessage("<InstantiatePlayer>");
                NicknamePanel.SetActive(false);

            }
            else
            {
                ErrorNickname.gameObject.SetActive(true);
            }
        }
        catch (System.Exception)
        {
            print("Exception");
        }
    }
}
