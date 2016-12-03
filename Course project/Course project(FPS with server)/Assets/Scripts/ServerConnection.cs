using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ServerConnection : MonoBehaviour
{
    static TcpClient client;

    public GameObject PlayerPrefab;
    GameObject PlayerGameObject;

    static bool instantiatePlayer = false;

    public ServerConnection(string connectIP, int Port)
    {
        client = new TcpClient();
        client.Connect(IPAddress.Parse(connectIP), Port);
    }

    public void Work()
    {
        Thread clientListener = new Thread(Reader);
        clientListener.Start();
    }

    void Update()
    {
        if (instantiatePlayer)
        {
            InstantiatePlayer();
            instantiatePlayer = false;
        }
    }

    public new void SendMessage(string message)
    {
        message.Trim();
        byte[] Buffer = Encoding.UTF8.GetBytes(message);
        client.GetStream().Write(Buffer, 0, Buffer.Length);
        print(message);
    }

    public new void SendMessage(float posX, float posY, float posZ)
    {
        string message = "<PosX>" + posX.ToString() + "/<PosY>" + posY.ToString() + "/<PosZ>" + posZ.ToString();
        message.Trim();
        byte[] Buffer = Encoding.UTF8.GetBytes(message);
        client.GetStream().Write(Buffer, 0, Buffer.Length);
        print(message);
    }

    void Reader()
    {
        while (true)
        {
            NetworkStream networkStream = client.GetStream();
            List<byte> Buffer = new List<byte>();

            string[] message = null;

            while (networkStream.DataAvailable)
            {
                int readByte = networkStream.ReadByte();
                if (readByte > -1)
                {
                    Buffer.Add((byte)readByte);
                }

                if (Buffer.Count > 0)
                {
                    Manager.msg.Add(Encoding.UTF8.GetString(Buffer.ToArray()));

                    message = Encoding.UTF8.GetString(Buffer.ToArray()).Split('/');
                }
            }

            if (message != null)
            {
                foreach (var item in message)
                {
                    if (item.Contains("<PosX>"))
                    {
                        Player.Instance.PosX = Convert.ToSingle(item.Remove(0, 6));
                    }
                    else if (item.Contains("<PosY>"))
                    {
                        Player.Instance.PosY = Convert.ToSingle(item.Remove(0, 6));
                    }
                    else if (item.IndexOf("<PosZ>") > -1)
                    {
                        Player.Instance.PosZ = Convert.ToSingle(item.Remove(0, 6));
                    }

                    if (item == "<InstantiatePlayer>")
                    {
                        instantiatePlayer = true;
                    }
                }
            }
        }
    }

    public void InstantiatePlayer()
    {
        PlayerGameObject = Instantiate(PlayerPrefab);
        PlayerGameObject.gameObject.transform.position = new Vector3(Player.Instance.PosX, Player.Instance.PosY, Player.Instance.PosZ);
    }

    ~ServerConnection()
    {
        if (client != null)
        {
            client.Close();
        }
    }

    public class Player
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public string Name { get; set; }
        public bool Team { get; set; }

        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }


        private Player()
        {
        }
    }
}




