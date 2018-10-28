using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices; // C# calle Javascript

namespace Meguru
{
    [Serializable]
    public class Output
    {
        [DllImport("__Internal")]
        private static extern void SendFieldData(string str); // host: C# call Javascript

        [DllImport("__Internal")]
        private static extern void SendAgentData(string str); // client: C# call Javascript

        /*
        マスのサイズ,プレイヤーの位置,マスの点数,マスの状態
        */
        public Field field;
        public List<Agent> agents;

        public Output()
        {
            this.field = Field.ReadStatic();
            this.agents = Agent.ReadStatic();
        }

        public static void DataOutput(Output data)
        {
            Field field = data.field;
            List<Agent> agents = data.agents;
            string bondData;

            if (!Main.myTeam) // client
            {
                string agentData = agents[1].current.ToString() + "," + agents[2].current.ToString();
                SendAgentData(agentData);
                return;
            }

            bondData = field.Height.ToString() + ":" + field.Width.ToString() + ",";

            foreach (Agent agent in agents)
                bondData += agent.current.x.ToString() + ":" + agent.current.y.ToString() + ":";
            bondData = bondData.Remove(bondData.Length - 1);
            bondData += ",";

            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    bondData += field.tiles[x, y].point.ToString();
                    if (x != field.Width - 1)
                        bondData += ":";
                }
                if (y != field.Height - 1)
                    bondData += "/";
            }
            bondData += ",";

            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    bondData += field.tiles[x, y].stat.ToString();
                    if (x != field.Width - 1)
                        bondData += ":";
                }
                if (y != field.Height - 1)
                    bondData += "/";
            }

            GetData.staticData = bondData;
            SendFieldData(bondData);
        }
    }
}