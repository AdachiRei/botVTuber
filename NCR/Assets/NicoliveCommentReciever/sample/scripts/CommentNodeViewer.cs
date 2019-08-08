using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NicoliveCommentReciever.sample
{
    /// <summary>
    /// コメント一覧ノード要素
    /// </summary>
    public class CommentNodeViewer : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Toggle premiumToggle;
        [SerializeField]
        private Toggle staffToggle;
        [SerializeField]
        private Text messageText;
        [SerializeField]
        private RawImage iconImage;

        public void Register(Comment comment)
        {
            nameText.text = !string.IsNullOrEmpty(comment.NickName) ? comment.NickName : comment.Name;
            premiumToggle.isOn = comment.IsPremium;
            staffToggle.isOn = comment.IsStaff;
            messageText.text = comment.Message;

            if (!comment.Anonymity)
            {
                int userId;
                var result = int.TryParse(comment.UserId, out userId);
                if (result) StartCoroutine(UserIconCoroutine(userId));
            }
        }

        IEnumerator UserIconCoroutine(int userId)
        {
            var uri = string.Format(@"http://usericon.nimg.jp/usericon/{0}/{1}.jpg", userId / 10000, userId);
            var www = new WWW(uri);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                yield break;
            }

            iconImage.texture = www.texture;

        }

    }
}
