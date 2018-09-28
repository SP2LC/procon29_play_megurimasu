using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Meguru
{
    public class Field
    {
        public Tile[,] tiles;
        private int width, height;

        Field(int width = 20, int height = 20)
        {
            this.width = width;
            this.height = height;
            this.tiles = new Tile[width, height];
        }

        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }

        // 静的ファイルの読み込み関数 
        /*
        @return Field 読み込まれた値がセットされたFieldインスタンス
         */
        public static Field ReadStatic()
        {
            Field returnField;

            // データを渡す
            var allText = ReadData.staticData;
            // 読み込みやすくするため，文字を置き換える
            allText = allText.Replace("/", ":");

            var splitData = allText.Split(',');

            /*
            @splitData[0] 横と縦の大きさ
             */
            var andHeightWidth = splitData[0].Split(':');

            returnField =
                new Field(
                    int.Parse(andHeightWidth[0]),
                    int.Parse(andHeightWidth[1])
                );

            /*
            @splitData[2] タイルのポイント
            @splitData[3] タイルの状況(0:白, 1:赤, 2:青)
             */
            var points = Array.ConvertAll(splitData[2].Split(':'), int.Parse);
            var stats = Array.ConvertAll(splitData[3].Split(':'), int.Parse);
            var w = returnField.Width;
            var h = returnField.Height;

            // 二次元配列のインデックス進行を一次元に落とし込む計算式 = y * w + x
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    returnField.tiles[x, y] = new Tile(stat: stats[y * w + x], point: points[y * w + x]);

            return returnField;
        }
    }
}