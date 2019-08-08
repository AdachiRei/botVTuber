using UnityEngine;
using UnityEngine.UI;

namespace NicoliveCommentReciever.sample
{
    public class CommentNodeDispatcher : MonoBehaviour
    {
        [SerializeField]
        private NicoliveCommentRecieveComponent reciever;
        [SerializeField]
        private Transform commentNodeRoot;

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField]
        private GameObject nodePrefab;

        void Start()
        {
            reciever.MessageRecievedEvent += (sender, args) =>
            {
                var o = Instantiate(nodePrefab);
                o.transform.SetParent(commentNodeRoot, false);

                var v = o.GetComponent<CommentNodeViewer>();
                v.Register(args.Comment);

                scrollRect.verticalNormalizedPosition = 0;
            };
        }

    }
}
