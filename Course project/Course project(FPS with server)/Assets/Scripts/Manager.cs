using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static List<string> msg = new List<string>();
    ServerConnection con;

    public GameObject NamePrefab;
    public GameObject RedTeamList;
    public GameObject GreenTeamList;

    public GameObject NicknamePanel;
    public GameObject ChooseTeamPanel;
    public Text ErrorNickname;
    public InputField NicknameInputField;

    void Start()
    {
        NicknamePanel.SetActive(true);
        con = new ServerConnection("127.0.0.1", 11000);
        con.Work();
    }
    
    private void Update()
    {
    }

    public void ChooseTeamButton()
    {
        if (NicknameInputField.text.Length != 0)
        {
            //con.SendMessage("<MyNickname>" + NicknameInputField.text);
            ServerConnection.Player.Instance.Name = NicknameInputField.text;

            ErrorNickname.gameObject.SetActive(false);
            NicknamePanel.SetActive(false);
            ChooseTeamPanel.SetActive(true);
        }
        else
        {
            ErrorNickname.gameObject.SetActive(true);
        }
    }

    public void RedTeam()
    {
        GameObject myName = Instantiate(NamePrefab);
        myName.transform.SetParent(GameObject.Find("RedTeamList").transform);
        myName.GetComponent<Text>().text = ServerConnection.Player.Instance.Name;

        foreach (var item in GreenTeamList.GetComponentsInChildren<Text>())
        {
            if (item.text == ServerConnection.Player.Instance.Name)
            {
                Destroy(item.gameObject);
            }
        }

        int CheckValue = 0;

        for (int i = 0; i < RedTeamList.GetComponentsInChildren<Text>().Length; i++)
        {
            if (RedTeamList.GetComponentsInChildren<Text>()[i].text == myName.GetComponent<Text>().text)
            {
                CheckValue++;

                if (CheckValue > 1)
                {
                    Destroy(RedTeamList.GetComponentsInChildren<Text>()[i].gameObject);
                    CheckValue = 1;
                }
            }
        }
    }

    public void GreenTeam()
    {
        GameObject myName = Instantiate(NamePrefab);
        myName.transform.SetParent(GameObject.Find("GreenTeamList").transform);
        myName.GetComponent<Text>().text = ServerConnection.Player.Instance.Name;

        foreach (var item in RedTeamList.GetComponentsInChildren<Text>())
        {
            if (item.text == ServerConnection.Player.Instance.Name)
            {
                Destroy(item.gameObject);
            }
        }

        int CheckValue = 0;

        for (int i = 0; i < GreenTeamList.GetComponentsInChildren<Text>().Length; i++)
        {
            if (GreenTeamList.GetComponentsInChildren<Text>()[i].text == myName.GetComponent<Text>().text)
            {
                CheckValue++;

                if (CheckValue > 1)
                {
                    Destroy(GreenTeamList.GetComponentsInChildren<Text>()[i].gameObject);
                    CheckValue = 1;
                }
            }
        }
    }

    public void Play()
    {
        //int countPlayers = 0;
        //for (int i = 0; i < RedTeamList.GetComponentsInChildren<Text>().Length; i++)
        //{
        //    countPlayers++;
        //}

        //for (int i = 0; i < RedTeamList.GetComponentsInChildren<Text>().Length; i++)
        //{
        //    countPlayers++;
        //}

        //for (int i = 0; i < countPlayers; i++)
        {
            con.SendMessage("<PosX>111/<PosY>111/<PosZ>111/<InstantiatePlayer>/");
        }
    }
}
