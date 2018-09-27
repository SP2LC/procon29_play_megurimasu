using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Meguru
{
    public class Action
    {
        public Way way; // 方向
        public Motion motion;

        public Action(Way way, Motion motion)
        {
            this.way = way;
            this.motion = motion;
        }

        // それぞれの行動を読み込む
        public static List<Action> ReadDynamic(List<Agent> agents)
        {
            var actions = new List<Action>();

            string allText;

            // ファイル読み込み
            allText = File.ReadAllText(PathSet.dynamicPath);
            // 読み込みやすくするため，文字を置き換える
            allText = allText.Replace("[", ",").Replace("(", "").Replace(")", "").Replace("]", "");

            var splitData = allText.Split(':');

            var willAction0 = Array.ConvertAll(splitData[1].Split(','), int.Parse);
            var willAction1 = Array.ConvertAll(splitData[2].Split(','), int.Parse);

            var incrementCurrent0 = new Point(agents[0].current.x, agents[0].current.y);
            var incrementCurrent1 = new Point(agents[3].current.x, agents[3].current.y);

            for (int turn = 0; turn < int.Parse(splitData[0]); turn++)
            {
                actions.Add(IncrementToWay((new Point(willAction0[turn * 3], willAction0[turn * 3 + 1]) - incrementCurrent0), willAction0[turn * 3 + 2]));
                incrementCurrent0 += EnumToPoint(actions[turn * 2]); // 1手目より先をみるために，行動したことにして，移動させる
                actions.Add(IncrementToWay((new Point(willAction1[turn * 3], willAction1[turn * 3 + 1]) - incrementCurrent1), willAction1[turn * 3 + 2]));
                incrementCurrent1 += EnumToPoint(actions[turn * 2 + 1]);
            }

            return actions;
        }

        // 差分とマスに対しての行動からActionを組み立てる
        public static Action IncrementToWay(Point increment, int motionId)
        {
            Motion motion;

            switch (motionId)
            {
                case 1:
                    motion = Motion.MOVE;
                    break;
                case 2:
                    motion = Motion.BREAK;
                    break;
                case 0:
                default:
                    motion = Motion.STAY;
                    break;
            }

            switch (increment.ToString())
            {
                case "(0, -1)":
                    return new Action(Way.FRONT, motion);

                case "(1, -1)":
                    return new Action(Way.FRONTRIGHT, motion);

                case "(1, 0)":
                    return new Action(Way.RIGHT, motion);

                case "(1, 1)":
                    return new Action(Way.BACKRIGHT, motion);

                case "(0, 1)":
                    return new Action(Way.BACK, motion);

                case "(-1, 1)":
                    return new Action(Way.BACKLEFT, motion);

                case "(-1, 0)":
                    return new Action(Way.LEFT, motion);

                case "(-1, -1)":
                    return new Action(Way.FRONTLEFT, motion);

                case "(0, 0)":
                default:
                    return new Action(Way.STAY, motion);

            }
        }

        // enumの値をPointに変換
        public static Point EnumToPoint(Action latestAction)
        {
            // motionがMOVEの時のみAgentの位置が変更される
            switch ((int)latestAction.motion)
            {
                case 1:
                    break;
                case 2:
                case 0:
                default:
                    return new Point(0, 0);

            }

            switch ((int)latestAction.way)
            {
                case 1:
                    return new Point(0, -1);

                case 2:
                    return new Point(1, -1);

                case 3:
                    return new Point(1, 0);

                case 4:
                    return new Point(1, 1);

                case 5:
                    return new Point(0, 1);

                case 6:
                    return new Point(-1, 1);

                case 7:
                    return new Point(-1, 0);

                case 8:
                    return new Point(-1, -1);

                case 0:
                default:
                    return new Point(0, 0);

            }
        }
    }
}