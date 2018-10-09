using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

namespace Meguru
{
    /*
    単に座標をあらわす
    @x x座標
    @y y座標
     */
    public class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a.x != b.x || a.y != b.y;
        }
    }

    public enum Way : int
    {
        STAY = 0, // 以後1ずつオートインクリメント
        FRONT,
        FRONTRIGHT,
        RIGHT,
        BACKRIGHT,
        BACK,
        BACKLEFT,
        LEFT,
        FRONTLEFT
    }

    public enum Motion : int
    {
        STAY = 0,
        MOVE = 1,
        BREAK = 2
    }

    public class Main : MonoBehaviour
    {
        private float heiSym, widSym; // 縦,横の比率を画面中央を中心とさせる為
        public Canvas canvas; // キャンバス
        private GameObject tile; // インスタンス化されたタイル
        public GameObject grayTile; // prefab
        public GameObject blueAgent; // prefab
        public GameObject redAgent; // prefab
        public GameObject blueTile; // prefab
        public GameObject redTile; // prefab
        public GameObject selectTile; // prefab
        private GameObject allGrayTile; // Hierarchy
        private GameObject allBlueAgent; // Hierarchy
        private GameObject allRedAgent; // Hierarchy
        private GameObject allBlueTile; // Hierarchy
        private GameObject allRedTile; // Hierarchy
        private GameObject allSelectTile; // Hierarchy
        private GameObject timerText; // Timer表示用Text
        private Vector3 tilePosition; // タイルの座標
        public GameObject pointTextPrefab; // prefab化されたテキスト
        private GameObject textInstant; // インスタンス化されたテキスト
        private Text pointText; // インスタンス化されたテキスト
        private RectTransform textRect; //テキストのRectTransform
        private Vector2 textPosition; // テキストの最終的な位置

        private string clickedGameObject; // クリックしたゲームオブジェクト
        private float timeElapsed; // Update関数のタイマー処理に利用
        Field field; // フィールド(タイルの集合体)
        List<Agent> agents; // Agentの情報s
        Output data; // 再入力から出力する用のデータの集合体
        private bool dataInputIsFinish; // 再入力用のデータ入力が完了したかどうか
        private int idData; // Output用のデータ作成のAgentの識別に用いる
        private bool createFieldIsFinish; // フィールドを作成したかどうか
        private int[] selectCoordinate; // 選択した座標 [0]:x, [1]:y


        // Use this for initialization
        void Start()
        {
            allGrayTile = GameObject.Find("GrayTile");
            allBlueAgent = GameObject.Find("BlueAgent");
            allRedAgent = GameObject.Find("RedAgent");
            allBlueTile = GameObject.Find("BlueTile");
            allRedTile = GameObject.Find("RedTile");
            allSelectTile = GameObject.Find("SelectTile");

            timerText = GameObject.Find("Canvas/Timer");

            createFieldIsFinish = false;
            selectCoordinate = new int[2] { -1, -1 };

            makeField(); //フィールド作成
        }

        // Update is called once per frame
        void Update()
        {
            timeElapsed += Time.deltaTime;
            timerText.GetComponent<Text>().text = timeElapsed.ToString();

            //-----------出力-----------//
            if (Input.GetMouseButtonDown(0))
            {
                // クリックしたものの取得
                clickedGameObject = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit2d)
                {
                    clickedGameObject = hit2d.transform.gameObject.name;
                }

                // 操作
                if (clickedGameObject != null)
                {
                    if (clickedGameObject.Contains("redagent")) // 赤チーム
                    {
                        clickedGameObject = clickedGameObject.Replace("redagent_", "");
                    }
                    else if (clickedGameObject.Contains("blueagent")) // 青チーム
                    {
                        clickedGameObject = clickedGameObject.Replace("blueagent_", "");
                    }

                    string compare = selectCoordinate[0] + "," + selectCoordinate[1];
                    if (compare.Equals(clickedGameObject))
                    {
                        selectMode(false);
                        selectCoordinate = new int[2] { -1, -1 };
                        return;
                    }
                    // クリックしたオブジェクトの座標を区切ってint[]としてselectCoordinateに渡す
                    selectCoordinate = clickedGameObject.Split(',').Select(s => int.Parse(s)).ToArray();
                    selectMode(true);
                }
                else
                {
                    Debug.Log("nullである");
                }
            }

            if (8f < timeElapsed) // Reload
            {
                timeElapsed = 0;
                if (!createFieldIsFinish) // フィールドがない
                    return;

                StartCoroutine(ReloadCoroutine());
            }
            //-------------------------//
        }

        private IEnumerator ReloadCoroutine()
        {
            ConnectServer con = new ConnectServer(); // データのロード
            yield return StartCoroutine(con.Connect());

            TileReset(); // タイルのリセット
            AgentPositionReset(); // Agentタイルのリセット

            field = Field.ReadStatic(); // 再読み込み
            agents = Agent.ReadStatic(); // 再読み込み

            if (field == null) // 情報が読み込まれていない
                yield break;

            AddActionToAgent(Action.ReadDynamic(agents)); // agentsにactionを設定

            AgentPosition(); // Agentの位置更新
            TileUpdate(); // タイルのアップデート
            TextUpdate(); // テキストのアップデート
        }

        //---------------------------------出力---------------------------------//

        // 移動するマスの選択モード
        private void selectMode(bool orMode)
        {
            if (!orMode)
            {
                for (int x = selectCoordinate[0] - 1; x <= selectCoordinate[0] + 1; x++)
                {
                    for (int y = selectCoordinate[1] - 1; y <= selectCoordinate[1] + 1; y++)
                    {
                        if (x == selectCoordinate[0] && y == selectCoordinate[1])
                        {
                            continue;
                        }
                        GameObject sTile = GameObject.Find("selecttile_" + x + "," + y);
                        sTile.GetComponent<SpriteRenderer>().enabled = false;
                        sTile.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                return;
            }

            for (int x = selectCoordinate[0] - 1; x <= selectCoordinate[0] + 1; x++)
            {
                for (int y = selectCoordinate[1] - 1; y <= selectCoordinate[1] + 1; y++)
                {
                    if (x == selectCoordinate[0] && y == selectCoordinate[1])
                    {
                        continue;
                    }
                    GameObject sTile = GameObject.Find("selecttile_" + x + "," + y);
                    sTile.GetComponent<SpriteRenderer>().enabled = true;
                    sTile.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }

        // フィールド作成
        private void makeField()
        {
            Debug.Log(ReadData.staticData);
            if (createFieldIsFinish) // フィールドが作成済み
                return;

            field = Field.ReadStatic(); // Field情報の読み込み
            agents = Agent.ReadStatic(); // Agent情報の読み込み

            if (field == null) // 情報が読み込まれていない
                return;

            AddActionToAgent(Action.ReadDynamic(agents)); // agentsにactionを設定

            CreateField(); // マスの生成
            AgentPosition(); // Agentの位置更新
            TileUpdate(); // タイルのアップデート
            AgentPosition(); // Agentの位置情報の追加
            createFieldIsFinish = true;
        }

        // それぞれのAgentの読み込んだActionを設定する
        private void AddActionToAgent(List<Action> actions)
        {
            var id = 0;
            foreach (Action action in actions)
            {
                agents[id].actions.Add(action);
                if (id == 0)
                {
                    id = 3;
                }
                else
                    id = 0;
            }
        }

        // タイル情報の更新
        private void TileUpdate()
        {
            for (int w = 0; w < field.Width; w++)
            {
                for (int h = 0; h < field.Height; h++)
                {
                    string current = w + "," + h;
                    switch (field.tiles[w, h].stat)
                    {
                        case 0:
                            break;
                        case 1:
                            GameObject.Find("redtile_" + current).GetComponent<SpriteRenderer>().enabled = true;
                            break;
                        case 2:
                            GameObject.Find("bluetile_" + current).GetComponent<SpriteRenderer>().enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // タイルの情報をリセット(ファイルを更新した際に必要)
        private void TileReset()
        {
            for (int w = 0; w < field.Width; w++)
            {
                for (int h = 0; h < field.Height; h++)
                {
                    string current = w + "," + h;
                    switch (field.tiles[w, h].stat)
                    {
                        case 0:
                            break;
                        case 1:
                            GameObject.Find("redtile_" + current).GetComponent<SpriteRenderer>().enabled = false;
                            break;
                        case 2:
                            GameObject.Find("bluetile_" + current).GetComponent<SpriteRenderer>().enabled = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Agentの場所を更新
        private void AgentPosition()
        {
            foreach (Agent agent in agents)
            {
                string current = agent.current.x + "," + agent.current.y;
                switch (agent.id)
                {
                    // タイルそれぞれの描画設定のオンオフで判別する GetComponent<SpriteRenderer>()
                    case 0:
                    case 3:
                        GameObject.Find("redagent_" + current).GetComponent<SpriteRenderer>().enabled = true;
                        GameObject.Find("redtile_" + current).GetComponent<SpriteRenderer>().enabled = true;
                        GameObject.Find("redagent_" + current).GetComponent<BoxCollider2D>().enabled = true;
                        break;
                    case 1:
                    case 2:
                        GameObject.Find("blueagent_" + current).GetComponent<SpriteRenderer>().enabled = true;
                        GameObject.Find("bluetile_" + current).GetComponent<SpriteRenderer>().enabled = true;
                        GameObject.Find("blueagent_" + current).GetComponent<BoxCollider2D>().enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        // Agentの場所をリセット(移動していなくなった，ファイルの読み込みで場所が変わった時)
        private void AgentPositionReset()
        {
            foreach (Agent agent in agents)
            {
                string current = agent.current.x + "," + agent.current.y;
                switch (agent.id)
                {
                    // タイルそれぞれの描画設定のオンオフで判別する GetComponent<SpriteRenderer>()
                    case 0:
                    case 3:
                        GameObject.Find("redagent_" + current).GetComponent<SpriteRenderer>().enabled = false;
                        GameObject.Find("redagent_" + current).GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    case 1:
                    case 2:
                        GameObject.Find("blueagent_" + current).GetComponent<SpriteRenderer>().enabled = false;
                        GameObject.Find("blueagent_" + current).GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        // テキストの更新
        private void TextUpdate()
        {
            for (int h = 0; h < field.Height; h++)
            {
                for (int w = 0; w < field.Width; w++)
                {
                    textInstant = GameObject.Find("point_" + w + "," + h);
                    pointText = textInstant.GetComponent<Text>();
                    pointText.text = field.tiles[w, h].point.ToString();
                }
            }
        }

        // マスの生成
        private void CreateField()
        {
            /*
            半分の1/2 * マス目の比率を合わせるための1/3で1/6
             */
            widSym = (float)(field.Width - 1) / 4.5f;
            heiSym = (float)(field.Height - 1) / 4.5f;

            for (int h = 0; h < field.Height; h++)
            {
                for (int w = 0; w < field.Width; w++)
                {
                    tilePosition = new Vector3((float)w / 2.25f - widSym, (float)(9 - h) / 2.25f - heiSym, 500);
                    // グレーのタイル
                    tile = Instantiate(grayTile, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allGrayTile.transform, false);
                    GrayTileRename(tile, w, h); // タイルの名前を変更

                    // ブルーAgentのタイル
                    tile = Instantiate(blueAgent, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allBlueAgent.transform, false);
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                    tile.GetComponent<BoxCollider2D>().enabled = false;
                    BlueAgentRename(tile, w, h); // タイルの名前を変更

                    // レッドAgentのタイル
                    tile = Instantiate(redAgent, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allRedAgent.transform, false);
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                    tile.GetComponent<BoxCollider2D>().enabled = false;
                    RedAgentRename(tile, w, h); // タイルの名前を変更

                    // ブルーのタイル
                    tile = Instantiate(blueTile, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allBlueTile.transform, false);
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                    BlueTileRename(tile, w, h); // タイルの名前を変更

                    // レッドのタイル
                    tile = Instantiate(redTile, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allRedTile.transform, false);
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                    RedTileRename(tile, w, h); // タイルの名前を変更

                    // プレイヤーが選択中のタイル
                    tile = Instantiate(selectTile, tilePosition, Quaternion.identity);
                    tile.transform.SetParent(allSelectTile.transform, false);
                    tile.GetComponent<SpriteRenderer>().enabled = false;
                    tile.GetComponent<BoxCollider2D>().enabled = false;
                    SelectTileRename(tile, w, h); // タイルの名前を変更

                    // ポイント関連
                    CreatePoint(tilePosition, w, h); // ポイントの書かれた文字の生成
                    PointRename(pointText, w, h); // ポイントの書かれた文字の名前を変更
                }
            }
        }

        // マスに点数を表示
        private void CreatePoint(Vector3 vec3, int w, int h)
        {
            textInstant = Instantiate(pointTextPrefab);
            textInstant.transform.SetParent(canvas.transform, false);
            pointText = textInstant.GetComponent<Text>();
            pointText.text = field.tiles[w, h].point.ToString();

            textRect = textInstant.GetComponent<RectTransform>();
            textRect.localPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, vec3); // uGUIは座標のタイプが違うっぽい(調べてない)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), textRect.localPosition, Camera.main, out textPosition);
            textRect.localPosition = textPosition;
        }

        /*
        ここから下はのちのちコードで記憶するかも
         */
        // マスの名前を座標に変更 "graytile_w_h"
        private void GrayTileRename(GameObject tile, int width, int height)
        {
            tile.name = "graytile_" + width + "," + height;
        }

        // マスの名前を座標に変更 "blueagent_w_h"
        private void BlueAgentRename(GameObject tile, int width, int height)
        {
            tile.name = "blueagent_" + width + "," + height;
        }

        // マスの名前を座標に変更 "redagent_w_h"
        private void RedAgentRename(GameObject tile, int width, int height)
        {
            tile.name = "redagent_" + width + "," + height;
        }

        // マスの名前を座標に変更 "bluetile_w_h"
        private void BlueTileRename(GameObject tile, int width, int height)
        {
            tile.name = "bluetile_" + width + "," + height;
        }

        // マスの名前を座標に変更 "redtile_w_h"
        private void RedTileRename(GameObject tile, int width, int height)
        {
            tile.name = "redtile_" + width + "," + height;
        }

        // マスの名前を座標に変更 "selecttile_w_h"
        private void SelectTileRename(GameObject tile, int width, int height)
        {
            tile.name = "selecttile_" + width + "," + height;
        }

        // ポイントが書かれたテキストの名前を座標に変更 "point_h_w"
        private void PointRename(Text point, int width, int height)
        {
            point.name = "point_" + width + "," + height;
        }
        //---------------------------------------------------------------------//
    }
}
