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

        public void InputFieldData(string fieldData)
        {
            if (Main.startField == 0)
            {
                staticData = fieldData;
                Main.startField = 1;
            }
        }

        public void SetTeam(string myTeam)
        {
            if (myTeam == "true")
            {
                Main.myTeam = true;
            }
            else
            {
                Main.myTeam = false;
            }
        }

        public void GetClientData(string agentData)
        {
            Debug.Log("1");
            string[] splitAgentData = agentData.Split(',');
            Debug.Log("2");

            Main.data.agents[1].current.x = int.Parse(splitAgentData[0]);
            Debug.Log("3");
            Main.data.agents[1].current.y = int.Parse(splitAgentData[1]);
            Debug.Log("4");
            Main.data.agents[2].current.x = int.Parse(splitAgentData[2]);
            Debug.Log("5");
            Main.data.agents[2].current.y = int.Parse(splitAgentData[3]);
            Debug.Log("6");

            Main.inputFinished[1] = true;
            Debug.Log("inpu:  ==" + Main.inputFinished[1]);
        }

        public void GetFieldData(string fieldData)
        {
            staticData = fieldData;
            Main.needUpdate = true;
        }
    }
}
