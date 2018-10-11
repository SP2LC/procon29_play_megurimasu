using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

namespace Meguru
{
    [Serializable]
    public class Agent
    {
        public Point current; // 現在の位置
        public int id;
        public static int agentMax = 4;

        Agent(int id, Point first)
        {
            this.current = first;
            this.id = id;
        }

        public static List<Agent> ReadStatic()
        {
            Point first;
            var agents = new List<Agent>();

            string allText;
            // データ渡し
            allText = ReadData.staticData;

            var target = allText.Split(',')[1];

            /*
            @splitData[1] Agentの位置
            */
            var agentPosition = Array.ConvertAll(target.Split(':'), s => int.Parse(s));

            for (int id = 0; id < agentMax; id++)
            {
                first = new Point(agentPosition[id * 2], agentPosition[id * 2 + 1]);
                agents.Add(new Agent(id, first));
            }

            return agents;
        }
    }
}