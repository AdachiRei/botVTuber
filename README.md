# botVTuber
botにyoutuber・ニコ生主をやらせるためのシステムです。現在は主にコメントからのリアルタイムマルコフ連鎖使用。


-------------------------------------------------------------------------------------------------------------------


◎ソフトウェア/環境等


　このプログラムをいじる/動かすのに必要なソフトウェアや環境を以下に記します。


・Visual Studio 2017(*以下VS)
コードをいじるために必要なエディター
 
・.Net Framework ver4.0.3
プログラムを走らせるのに必要な実行環境 

・Youtube Data API
Youtube Liveのコメントを取得するのに使います

・やります！アンコちゃん
ニコニコからコメント持ってくるときに使います

・anko2unity
アンコちゃんに入れるプラグイン

・Nicolive Comment Reciever(*一部配布)
Unityに入れるパッケージ

・MarkovInu(*配布)
持ってきたコメントから文章を生成するプログラム

・mecab
形態素解析ライブラリ
 

 *VSやUnity等は現状他バージョンでの動作確認は行えていません。

-------------------------------------------------------------------------------------------------------------------


◎ファイルの説明


・MarkovInu-master
マルコフ連鎖の本体部分です

・MarkovInu-master_youtube
youtubeからのコメント取得に関する部分です

・NCR
ニコ生からのコメント取得に関する部分です。

・result.txt
マルコフ連鎖で生成された文章が保存されます

・sample.txt
 取得したコメントが保存されます

・NicoliveCommentReciever.unitypackage
  メンテナンス用なのであまり気にしなくて大丈夫です

-------------------------------------------------------------------------------------------------------------------


◎設定方法


・先にVSとUnityをインストールし環境を整えておきます。

VS；https://docs.microsoft.com/ja-jp/visualstudio/install/install-visual-studio?view=vs-2019
(VS 2017の場合；https://docs.microsoft.com/ja-jp/visualstudio/releasenotes/vs2017-relnotes)
Unity；https://unity3d.com/jp/get-unity/download

からダウンロード・インストールができます。


・mecabも先にインストールしておきます。

taku910.github.io/mecab/#download
mecabはインストールするだけでOKです。


・Youtube Data API（ニコ生のみの場合不要）

以下の記事が参考になると思います
Youtube Data APIを使用して、Youtube Liveのコメントを取得する
https://qiita.com/MCK9595/items/fdbd543ff938febcd136


・やります！アンコちゃんの設定（youtubeのみの場合不要）

yarimasu.ankochan.net
　からダウンロード・インストールします。
https://qiita.com/toRisouP/items/52c0701147dcbdeb4b9df
　を参考に準備を進めます。

上記サイトの"anko2unity"をダウンロードします。
ダウンロードができたら、"anko2unity"はアンコちゃんのプラグインディレクトリに入れます。

プラグインに格納出来たらアンコちゃんを立ち上げ
、すぐにブラウザの選択画面が表示されるので、良く使用するブラウザを選択してください。
選択したブラウザでニコ生にログインします。
ログインしたまま、取得したい動画のURLの"lv数字"の部分をコピーし、アンコちゃん上部の放送URL欄にペーストします。
最後に、上部のプラグインタブから"anko2unity"を選択し、出てきたウィンドウのPort欄に"17305"と入力してください。
これでアンコちゃんの設定は完了です。


・Unityの設定（youtubeのみの場合不要）

アンコちゃん同様、
https://qiita.com/toRisouP/items/52c0701147dcbdeb4b9df
を参考に"NicoliveCommentReciever"をダウンロードします。
上記サイトから"NicoliveCommentReciever.unitypackage"をダウンロードして
Unityにimportしてください。

また、適当なGameObjectを生成し、
今回配布した"NicoliveCommentRecieveCompornent.cs"を貼り付けてください。

このときInspectorに表示されるHostの欄に"127.0.0.1"、Portの欄に"17305"と入力してください。


・MarkovInu(MarkovInu-master内)の設定

事前にコメントを格納するテキストファイルをどこかに作っておきます。(ex."sample.txt")

配布の"MarkovInu-master"をダウンロードし、MarkovInuフォルダ下の"MarkovInu.sln"を開きます。
22行目のconst string pathの""内に先ほど作ったテキストファイルのパスを入れます。
27行目には形態素解析ライブラリのディレクトリ(-----\MarkovInu-master\MarkovInu\MarkovInu\dic\ipadic)
を入力します。
45行目sleep()内はミリ秒を表し、文章生成の間隔を変えられます。
61行目n = は文章生成素材とするコメント数を表しています。
109行目のStreamWriter()で文章生成の結果を格納するテキストファイルを指定します。
そしてビルド。

生成された文章は毎回クリップボードにコピーされるので、Softalk、棒読みちゃん等でクリップボード監視を
使うと合成音声ソフトに読み上げさせることができます。

また、"MarkovKey.cs"の18行目N = の数字を変えることでマルコフ連鎖の次数を変更できます。
（増やすとよりしっかりした文になりますが元のコメントそのままになりがちです）


・MarkovInu(MarkovInu-master_yutube)の設定（ニコ生のみの場合不要）

22行目の  const string path = @"C:\botVTuber\sample.txt"; カッコ内にsample.txtのpass
27行目の DicDir = @"C:\botVTuber\MarkovInu-master\MarkovInu\MarkovInu\dic\ipadic" カッコ内に形態素解析ライブラリのpass
36行目の ApiKey = "ここにYoutube Data APIのキー" のカッコ内にYoutube Data APIのキーを入れてください。
40行目の   string liveChatId = GetliveChatID("ここにYoutube Liveの番組ID", youtubeService);
 のカッコ内にYoutube Liveの番組ID（IDだけ）を入れてください。
できたらビルド。

-------------------------------------------------------------------------------------------------------------------

◎使い方

※ニコ生の場合

（やりますアンコちゃん、Unity、MarkovInu(仮)の設定を済ませた後）
SampleScene.unity
 (NCR>Assets>NicoliveCommentReciver>sample>SampleScene.unity)
を起動し、Unityの再生ボタン、
 MarkovInu.sln(MarkovInu-master>MarkovInu>MarkovInu.sln)
の開始ボタンの順に押すと動きます。


※Youtubeの場合

MarkovInu.sln(MarkovInu-master_yutube>MarkovInu>MarkovInu.sln)の開始ボタン
MarkovInu.sln(MarkovInu-master>MarkovInu>MarkovInu.sln)の開始ボタン
両方を押すと動きます。


[共通]

この状態だと次々とresult.txtとsample.txtの中身が更新されていくだけなので、読み上げをさせたい場合は
棒読みちゃんやSoftalk、VOICEROID、唄詠等を起動しクリップボード監視をONにします。

-------------------------------------------------------------------------------------------------------------------

◎著者
TopStick,missile39,himukai-takumi

-------------------------------------------------------------------------------------------------------------------

◎Thanks

anko2Unity:by TORISOUP
https://github.com/TORISOUP/anko2Unity

NicoliveCommentReciever:by TORISOUP
https://github.com/TORISOUP/NicoliveCommentReciever

MarkovInu:by kakunpc
https://github.com/kakunpc/MarkovInu
