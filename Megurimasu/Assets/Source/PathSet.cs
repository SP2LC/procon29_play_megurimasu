using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Meguru
{
    public class PathSet : MonoBehaviour
    {
        public static string staticPath; // 静的なファイルのパス(固定されたデータ)
        public static string dynamicPath;

        // Use this for initialization
        void Start()
        {
            staticPath = "file_for_competition.txt";
            dynamicPath = "output_format.txt";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
