using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace
{
    public static class MatrixSource
    {
        public static List<Matrix4x4> GenerateMatrixFormText(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("try generate matrix with null or empty path");
                return new List<Matrix4x4>();
            }
            var jsonFile = Resources.Load<TextAsset>(path);
            if (jsonFile == null)
            {
                Debug.LogWarning("Can not find json text asset at path");
                return new List<Matrix4x4>();
            }
            if (string.IsNullOrEmpty(jsonFile.text))
            {
                Debug.LogWarning("try generate matrix with null or empty text data");
                return new List<Matrix4x4>();
            }
            
            return JsonConvert.DeserializeObject<List<Matrix4x4>>(jsonFile.text);
        }
    }
}