using UnityEngine;

namespace NicoliveCommentReciever
{
    public class NicoliveCommentRecieveComponent : MonoBehaviour
    {
        public string Host = "127.0.0.1";
        public int Port = 17305;
        public bool ConnectOnAwake;

        /// <summary>
        /// メッセージ受信時に発行されるイベント
        /// </summary>
        public event MessageRecievedHandler MessageRecievedEvent;
        public delegate void MessageRecievedHandler(object sender, CommentRecievedEventArgs e);

        private CommentClient client;

        void Awake()
        {
            Debug.Log("Awake");
            client = new CommentClient();
            if ( true)
            {
                Connect();
                Debug.Log("Awake2");
            }
        }

        void Update()
        {
            //Debug.Log("Update");
            if (!client.IsConnected) return;
            //Debug.Log("IsConnected");
            if (!client.IsCommentRecived()) return;
            //Debug.Log("IsCommentRecived");

            var comments = client.TakeRecievedComments();
            for (var i = 0; i < comments.Length; i++)
            {
                if (MessageRecievedEvent != null) MessageRecievedEvent(this, new CommentRecievedEventArgs(comments[i]));
                string str = comments[i].Message;
                Debug.Log(str);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Users\missi\Desktop\Markov20190602\sample.txt", true);
                sw.WriteLine(str);
                sw.Close();
            }
        }

        void OnDestroy()
        {
            client.Disconnect();
        }

        public void Connect()
        {
            client.Connect(Host, Port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }
    }
}
