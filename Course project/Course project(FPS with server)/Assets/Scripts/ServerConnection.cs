using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class ServerConnection : MonoBehaviour
{
    public static ServerConnection Instance;

    public GameObject EnemyPrefab;
    public GameObject EnemyGameObject;
    public static bool SynchronizedPosition = false;
    public static bool instantiatePlayer = false;
    public GameObject MyPlayer;

    static string Nickname;
    private const string host = "127.0.0.1";
    private const int port = 11000;
    static TcpClient client;
    static NetworkStream stream;

    public bool IsConnect { get; set; }

    private void Start()
    {
        IsConnect = false;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);

        Nickname = UnityEngine.Random.RandomRange(1, 100).ToString();
        client = new TcpClient();

        try
        {
            client.Connect(host, port);
            if (client.Connected)
            {
                IsConnect = true;
            }
            stream = client.GetStream();

            string message = Nickname;
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            Thread receivedThread = new Thread(new ThreadStart(ReceiveMessage));
            receivedThread.Start();
            print("enter " + Nickname);
        }
        catch (Exception ex)
        {
            print(ex.Message);
            //Disconnect();
        }
    }

    ~ServerConnection()
    {
        Disconnect();
    }

    void Update()
    {
        if (instantiatePlayer)
        {
            InstantiatePlayer();
            instantiatePlayer = false;
        }

        if (SynchronizedPosition)
        {
            SendMessage(MyPlayer.transform.position.x, MyPlayer.transform.position.y, MyPlayer.transform.position.z);
            SendMessage(MyPlayer.transform.rotation.x, MyPlayer.transform.rotation.y, MyPlayer.transform.rotation.z, MyPlayer.transform.rotation.w);
        }

        if (EnemyGameObject != null)
        {
            EnemyGameObject.transform.position = new Vector3(PosX, PosY, PosZ);
            EnemyGameObject.transform.eulerAngles = new Vector3(RotX, RotY, RotZ);
        }
    }

    public new void SendMessage(string message)
    {
        EncodingAndWrite(message);
    }

    public new void SendMessage(float posX, float posY, float posZ)
    {
        string message = "<Position>" + "/<PosX>" + posX.ToString() + "/<PosY>" + posY.ToString() + "/<PosZ>" + posZ.ToString();
        message.Trim();

        EncodingAndWrite(message);
    }

    public new void SendMessage(float rotX, float rotY, float rotZ, float rotW)
    {
        string message = "<Rotation>" + "/<RotX>" + rotX.ToString() + "/<RotY>" + rotY.ToString() + "/<RotZ>" + rotZ.ToString() + "/<RotW>" + rotW.ToString();
        message.Trim();

        EncodingAndWrite(message);
    }

    private void EncodingAndWrite(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    private void ReceiveMessage()
    {
        while (true)
        {
            try
            {
                byte[] data = new byte[1024];

                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                string message = builder.ToString();

                if (message.Contains("<InstantiatePlayer>"))
                {
                    instantiatePlayer = true;
                }
                else if (CheckMessage(message, "<Position>"))
                {
                    ParseMessage(message, "<Position>");
                }

                if (CheckMessage(message, "<Rotation>"))
                {
                    ParseMessage(message, "<Rotation>");
                }

                if (CheckMessage(message, "<EnemyHealth>"))
                {
                    ParseMessage(message, "<EnemyHealth>");
                }
            }
            catch (Exception ex)
            {
                print(ex);
                Disconnect();
            }
        }
    }

    private bool CheckMessage(string message, string value)
    {
        return message.Contains(value);
    }

    private void ParseMessage(string message, string typeMsg)
    {
        if (message != null)
        {
            switch (typeMsg)
            {
                case "<Position>":
                    PosX = FindValuesInMessage("PosX", message);
                    PosY = FindValuesInMessage("PosY", message);
                    PosZ = FindValuesInMessage("PosZ", message);
                    break;
                case "<Rotation>":
                    RotX = FindValuesInMessage("RotX", message);
                    RotX = FindValuesInMessage("RotY", message);
                    RotX = FindValuesInMessage("RotZ", message);
                    RotX = FindValuesInMessage("RotW", message);
                    break;
                case "<EnemyHealth>":
                    foreach (Match item in new Regex(Pattern("EnemyHealth")).Matches(message))
                    {
                        MyPlayer.GetComponent<PlayerHealth>().currentHealth = Convert.ToInt32(item.Value.Remove(0, 13));
                    }
                    break;
            }
        }
    }

    private float FindValuesInMessage(string pattern, string message)
    {
        foreach (Match item in new Regex(Pattern(pattern)).Matches(message))
        {
            return Convert.ToSingle(item.Value.Remove(0, 6));
        }

        return 0;
    }

    private string Pattern(string value)
    {
        return string.Format(@"<{0}>.?\d+.?\d+", value);
    }

    public void InstantiatePlayer()
    {
        EnemyGameObject = Instantiate(EnemyPrefab);
        EnemyGameObject.gameObject.transform.position = new Vector3(PosX, PosY, PosZ);
        EnemyGameObject.gameObject.transform.rotation = new Quaternion(RotX, RotY, RotZ, RotW);
        EnemyGameObject.name = Name;
        SynchronizedPosition = true;
    }

    void Disconnect()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
    }

    public static float PosX { get; set; }
    public static float PosY { get; set; }
    public static float PosZ { get; set; }

    public static float RotX { get; set; }
    public static float RotY { get; set; }
    public static float RotZ { get; set; }
    public static float RotW { get; set; }

    public static string Name { get; set; }
}




