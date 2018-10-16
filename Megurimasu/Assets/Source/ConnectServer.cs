using System.Collections;
using UnityEngine;

namespace Meguru
{
    public class ConnectServer
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator Connect()
        {
            string url = "http://megurimasu-sp2lc.skr.jp/test.txt";
            var www = new WWW(url);
            yield return www;
            GetData.staticData = www.text;
            Debug.Log("get is finished");
        }
    }
}