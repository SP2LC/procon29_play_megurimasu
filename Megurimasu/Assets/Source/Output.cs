using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Meguru
{
    class Output
    {
        /*
        マスのサイズ,プレイヤーの位置,マスの点数,マスの状態
        */
        public Field field;
        public List<Agent> agents;

        public string outputData;

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

            System.IO.StreamWriter output = new System.IO.StreamWriter(PathSet.staticPath, false);
            output.Write(bondData);
            output.Close();
        }
    }
}