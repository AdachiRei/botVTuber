using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NicoliveCommentReciever
{
    public class CommentRecievedEventArgs : EventArgs
    {
        public Comment Comment { get; private set; }
        public CommentRecievedEventArgs(Comment comment)
        {
            Comment = comment;
        }
    }

    /// <summary>
    /// コメビュに繋いで通信するクラス
    /// </summary>
    public class CommentClient
    {
        TcpClient tcpClient;

        byte[] buffer;

        object lockObject = new object();

        private Queue<Comment> recievedComments;

        public bool IsConnected
        {
            get { return this.tcpClient != null && this.tcpClient.Connected; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommentClient()
        {
            recievedComments = new Queue<Comment>();
            buffer = new byte[2048];
        }

        /// <summary>
        /// コメビュに接続を試みる
        /// </summary>
        public void Connect(string host, int port)
        {
            if (IsConnected)
            {
                Disconnect();
            }

            tcpClient = new TcpClient(host, port);
            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, CallBackBeginReceive, null);
        }

        private void CallBackBeginReceive(IAsyncResult ar)
        {
            try
            {
                var bytes = this.tcpClient.GetStream().EndRead(ar);

                if (bytes == 0)
                {
                    //接続断
                    Disconnect();
                    return;
                }

                var recievedMessage = Encoding.UTF8.GetString(buffer, 0, bytes);
                var comment = Comment.FromJson(recievedMessage);
                lock (lockObject)
                {
                    recievedComments.Enqueue(comment);
                }
                tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, CallBackBeginReceive, null);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public bool IsCommentRecived()
        {
            lock (lockObject)
            {
                return recievedComments.Count > 0;
            }
        }

        public Comment[] TakeRecievedComments()
        {
            var data = new Comment[recievedComments.Count];
            lock (lockObject)
            {
                recievedComments.CopyTo(data, 0);
                recievedComments.Clear();
            }
            return data;
        }

        public void Disconnect()
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                tcpClient.GetStream().Close();
                tcpClient.Close();
                tcpClient = null;
            }
            lock (lockObject)
            {
                recievedComments.Clear();
            }
        }
    }
}
