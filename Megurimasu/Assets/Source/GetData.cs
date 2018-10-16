using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Meguru
{
    public class GetData : MonoBehaviour
    {
        public static string staticData;
        public static string dynamicData;

        // Use this for initialization
        void Start()
        {
            staticData = "10:10,2:2:7:2:2:7:7:7,-3:-16:1:-8:13:13:-8:1:-16:-3/-12:1:-4:-15:15:15:-15:-4:1:-12/6:0:-9:-4:-11:-11:-4:-9:0:6/-3:11:3:0:-14:-14:0:3:11:-3/-16:-5:7:8:11:11:8:7:-5:-16/-16:-5:7:8:11:11:8:7:-5:-16/-3:11:3:0:-14:-14:0:3:11:-3/6:0:-9:-4:-11:-11:-4:-9:0:6/-12:1:-4:-15:15:15:-15:-4:1:-12/-3:-16:1:-8:13:13:-8:1:-16:-20,0:0:0:0:0:0:0:0:0:0/0:0:0:0:0:0:0:0:0:0/0:0:1:0:0:0:0:2:0:0/0:0:0:0:0:0:0:0:0:0/0:0:0:0:0:0:0:0:0:0/0:0:0:0:0:0:0:0:0:0/0:0:0:0:0:0:0:0:0:0/0:0:2:0:0:0:0:1:0:0/0:0:0:0:0:0:0:0:0:0/0:0:0:0:0:0:0:0:0:0";
            dynamicData = "3:(2,4)[1],(2,5)[1],(3,5)[1]:(7,6)[1],(8,7)[1],(8,8)[1]";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setTeam(bool myTeam)
        {
            Main.myTeam = myTeam;
        }

        public void getClientData(string agentData)
        {
            string[] splitAgentData = agentData.Split(',');

            Main.data.agents[1].current.x = int.Parse(splitAgentData[0]);
            Main.data.agents[1].current.y = int.Parse(splitAgentData[1]);
            Main.data.agents[2].current.x = int.Parse(splitAgentData[2]);
            Main.data.agents[2].current.y = int.Parse(splitAgentData[3]);

            Main.inputFinished[1] = true;
        }
    }
}
