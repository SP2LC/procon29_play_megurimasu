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
            string url = "http://mokapants.sakura.ne.jp/test.txt";
            var www = new WWW(url);
            yield return www;
            ReadData.staticData = www.text;
            Debug.Log("get is finished");
        }
    }
}