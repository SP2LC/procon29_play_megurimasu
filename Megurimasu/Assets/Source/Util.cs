using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Meguru
{
    public static class Util
    {
        public static T DeepCopy<T>(T original)
        {
            // シリアル化した内容を保持するメモリーストリームを生成
            MemoryStream stream = new MemoryStream();
            try
            {
                // バイナリ形式でシリアライズするためのフォーマッターを生成
                BinaryFormatter formatter = new BinaryFormatter();
                // コピー元のインスタンスをシリアライズ
                formatter.Serialize(stream, original);
                // メモリーストリームの現在位置を先頭に設定
                stream.Position = 0L;
                // メモリーストリームの内容を逆シリアル化
                return (T)formatter.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}