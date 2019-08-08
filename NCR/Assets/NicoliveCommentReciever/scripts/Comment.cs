using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NicoliveCommentReciever
{
    /// <summary>
    /// コメント情報
    /// </summary>
    public struct Comment
    {
        /// <summary>
        /// 匿名性
        /// </summary>
        public bool Anonymity { get; private set; }
        /// <summary>
        /// 投稿時刻
        /// </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// コマンド
        /// </summary>
        public string Mail { get; private set; }
        /// <summary>
        /// ユーザ名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// コメビュによって割り振られる名前
        /// </summary>
        public string NickName { get; private set; }
        /// <summary>
        /// コメント本文
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// コメント番号
        /// </summary>
        public int No { get; private set; }
        /// <summary>
        /// 投稿者属性
        /// </summary>
        public int Premium { get; private set; }
        /// <summary>
        /// スレッドID
        /// </summary>
        public int Thread { get; private set; }
        /// <summary>
        /// ユーザーID
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// コメント位置
        /// </summary>
        public int Vpos { get; private set; }
        /// <summary>
        /// 地域情報
        /// </summary>
        public string Locale { get; private set; }

        /// <summary>
        /// プレミアム会員であるか？
        /// </summary>
        public bool IsPremium
        {
            get { return Premium == 1; }
        }

        /// <summary>
        /// 放送者/運営コメントであるか？
        /// </summary>
        public bool IsStaff
        {
            get { return Premium == 3 || Premium == 6; }
        }

        public static Comment FromJson(string json)
        {
            var dto = JsonUtility.FromJson<CommentDataDto>(json);
            return new Comment
            {
                Anonymity = dto.Anonymity,
                Date = dto.Date,
                Mail = dto.Mail,
                Name = dto.Name,
                NickName =  dto.NickName,
                Message = dto.Message,
                No = dto.No,
                Premium = dto.Premium,
                Thread = dto.Thread,
                UserId = dto.UserId,
                Vpos = dto.Vpos,
                Locale = dto.Locale
            };
        }

        [Serializable]
        private struct CommentDataDto
        {
            public bool Anonymity;
            public DateTime Date;
            public string Mail;
            public string Name;
            public string NickName;
            public string Message;
            public int No;
            public int Premium;
            public int Thread;
            public string UserId;
            public int Vpos;
            public string Locale;
        }
    }
}
