using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MarkovInu.Markov;
using NMeCab;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;


namespace YoutubeCommentGetter
{
    
    internal class Program
    {
        // コメントを保存していくファイルのディレクトリ
        const string path = @"C:\botVTuber\sample.txt";//ここにsample.txtのpass

        // 形態素解析ライブラリのディレクトリ
        private static readonly MeCabParam MeCabParam = new MeCabParam
        {
            DicDir = @"C:\botVTuber\MarkovInu-master\MarkovInu\MarkovInu\dic\ipadic"//ここに形態素解析ライブラリのpass
        };

        private static MeCabTagger _meCabTagger;
        //[STAThread]
        static async Task Main(string[] args)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "ここにYoutube Data APIのキー"
            });


            string liveChatId = GetliveChatID("ここにYoutube Liveの番組ID", youtubeService);

            await GetLiveChatMessage(liveChatId, youtubeService, null);

        }

        static public string GetliveChatID(string videoId, YouTubeService youtubeService)
        {
            //引数で取得したい情報を指定
            var videosList = youtubeService.Videos.List("LiveStreamingDetails");
            videosList.Id = videoId;
            //動画情報の取得
            var videoListResponse = videosList.Execute();
            //LiveChatIDを返す
            foreach (var videoID in videoListResponse.Items)
            {
                return videoID.LiveStreamingDetails.ActiveLiveChatId;
            }
            //動画情報取得できない場合はnullを返す
            return null;
        }

        static public async Task GetLiveChatMessage(string liveChatId, YouTubeService youtubeService, string nextPageToken)
        {
            var liveChatRequest = youtubeService.LiveChatMessages.List(liveChatId, "snippet,authorDetails");
            liveChatRequest.PageToken = nextPageToken;

            System.IO.StreamWriter sw = new System.IO.StreamWriter(path,true);
            var liveChatResponse = await liveChatRequest.ExecuteAsync();
            foreach (var liveChat in liveChatResponse.Items)
            {
                try
                {
                    Console.WriteLine($"{liveChat.Snippet.DisplayMessage},{liveChat.AuthorDetails.DisplayName}");
                    sw.WriteLine(liveChat.Snippet.DisplayMessage);
                }
                catch { }

            }
            sw.Close();
            await Task.Delay((int)liveChatResponse.PollingIntervalMillis);


            await GetLiveChatMessage(liveChatId, youtubeService, liveChatResponse.NextPageToken);
        }
    }
}
