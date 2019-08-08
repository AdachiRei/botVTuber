using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MarkovInu.Markov;
using NMeCab;
using System.Windows.Forms;
using System.IO;
using System.Linq;






namespace MarkovInu
{
    // コメントを保存していくファイルのディレクトリ


    internal class Program
    {
        const string path = @"C:\botVTuber\sample.txt";   //ここにsample.txtのpass

        // 形態素解析ライブラリのディレクトリ
        private static readonly MeCabParam MeCabParam = new MeCabParam
        {
            DicDir = @"C:\botVTuber\MarkovInu-master\MarkovInu\MarkovInu\dic\ipadic"   //ここに形態素解析ライブラリのpass
        };

        private static MeCabTagger _meCabTagger;
        [STAThread]
        static void Main()
        {
            DateTime laset_update = System.IO.File.GetLastWriteTime(path);


            while (true)
            {
                //テキストファイル更新チェック
                DateTime LastWriteTime = System.IO.File.GetLastWriteTime(path);
                if (LastWriteTime > laset_update)
                {
                    Markov();
                }
                System.Threading.Thread.Sleep(15000);
            }

        }

        static void Markov()
        {
            _meCabTagger = MeCabTagger.Create(MeCabParam);
            var markovDic = new MarkovDictionary();

            // 正規表現(パターン検索)・・・この場合<>で囲まれた箇所にマッチする(+後述のreg.Replaceで文字を消す？)
            var reg = new Regex(@"<.*?>");



            // 読み込む行数をなんとなく先に定義
            var n = 10;

            // lock構文の準備
            object lockReed = new object();

            lock (lockReed)
            {
                // ここから先はコメントの読み込みの方法を色々試した努力の痕跡
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) //var file = new StreamReader(path)
                {
                    var lines = File.ReadAllLines(path);
                    string line;
                    //long pos = lines.Length - n;
                    //file.Seek(pos, SeekOrigin.Begin);
                    //while ((line = file.ReadLine() != null)
                    foreach (var ln in lines.Skip(lines.Length - n).Take(n))  // (最後の行-n行)目に跳んで、n行分読み込む
                    //for (int i = line.Length - 20; i < line.Length; i++)
                    {
                        line = reg.Replace(ln, "");
                        var marList = CheckMeCab(line);

                        markovDic.AddSentence(marList);
                    }
                }
            }




            var results = new List<string>();

            for (var i = 0; i < 100;)
            {
                var text = string.Join("", markovDic.BuildSentence());
                if (text.Length > 25) continue;

                results.Add(text);
                ++i;
            }

            // ここもlock構文の準備
            object lockWrite = new object();

            // マルコフ連鎖から返ってきた結果をresultファイルに投げる
            lock (lockWrite)
            {
                var r = results.FindMax(c => c.Length);
                //Console.WriteLine(r);
                using (var writer = new StreamWriter(@"C:\botVTuber\result.txt", true))  // StreamWriterメソッドの定義とresultファイルのディレクトリ
                {
                    writer.WriteLine(r + "\r\n");
                    Clipboard.SetText(r);
                }
            }
        }


        private static string[] CheckMeCab(string sentence)
        {
            var node = _meCabTagger.ParseToNode(sentence);
            var resultList = new List<string>();
            while (node != null)
            {
                if (node.CharType > 0)
                {
                    resultList.Add(node.Surface);
                }
                node = node.Next;
            }

            return resultList.ToArray();
        }
    }
}
