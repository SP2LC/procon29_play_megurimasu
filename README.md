# めぐります
## 概要
第29回高専プロコン競技部門の競技「巡りマス」をオンライン上でプレイできるシステムである。 
強い戦術を模索するために開発した。
大会公式HPに掲載された初期概要(9~14ページ)  
http://www.procon.gr.jp/wp-content/uploads//2016/12/e017e38ca89f7a0d04d6ad74319ffde0-1.pdf  

## 使用技術
- Unity
- SkyWay

## デベロッパー
- 岡江仁

## サポート
- 荻野先輩

## 使用方法
https://youtu.be/yLy4nk3rR5s

### 1.対戦相手との同期
自分の画面に表示された`Your Peer ID is: XXXXXXXXXXX`のX部を相手と共有し，`Connect to a peer: [ Someone else's id ]`に入力し，`Connect`。

### 2.フィールド生成
**※ゲームで使用するフィールド情報はprocon29_creatorを参照**  
SP2LCのメンバー用ではあるが，`procon29_play_megurimasu/Megurimasu/file_for_competition.txt`を読み解くと一応プレイ可能。
(`file_for_competition.txt`の中のものをコピペで一応プレイ可能)

### 3.行動方法
青か赤のAgentクリックして周りが黄色くなった方が自分チームのAgent。  
自チームのAgentをクリックするとAgentを原点に左右上下斜めのタイルが黄色く変わる。  
黄色い場所をクリックするとそのタイルに対して行動を起こす。  
空のタイルであれば移動，相手チームのタイルがあれば除去。  
操作が確定したらEnterキーを入力することで操作を確定。
双方がEnterキーを入力したらそのターンの情報が同期される。  
Agentを動かしたくない場合は，動かしたくないAgentの操作をせずにEnterキーを押せば動かない。

### 4.得点計算・タイマー機能について
開発期間に余裕がなかったため，**実装してない**。あくまでもこれは強い戦術を見つけるために使用することをお勧めする。
