using System;
using System.Collections.Generic;
using Code.Interfaces.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Utils.Extensions
{
    internal static class DataUtils
    {
        public static T GetData<T>(string path, T obj) where T : Object
        {
            if (obj == null)
            {
                obj = AssetPath.Load<T>(path);
            }
            
            if (obj is IData item)
                item.Path = path;

            return obj;
        }
        
        public static T[] GetDataList<T>(string[] paths, T[] objs) where T : Object
        {
            var objsNull = false;
            if (objs == null || objs.Length == 0)
            {
                objs = new T[paths.Length];
                objsNull = true;
            }

            if (objsNull)
            {
                for (var i = 0; i < paths.Length; i++)
                {
                    var path = paths[i];
                    var obj = AssetPath.Load<T>(path);
                    objs[i] = obj;

                    if (obj is IData item)
                        item.Path = path;
                }
            }

            return objs;
        }

        public static Dictionary<string, T> GetDataDict<T>(string path, Dictionary<string, T> obj) where T : Object
        {
            if (obj != null)
                return obj;
            
            obj = new Dictionary<string, T>();

            var datas = LoadAll<T>(path);

            foreach (var data in datas)
            {
                var dictData = data as IDictData;
                if (dictData == null)
                    throw new Exception("Локация не имеет интерфейса IDictData.");
                obj.Add(dictData.IDName, data);
            }

            return obj;
        }

        private static T[] LoadAll<T>(string path) where T : Object =>
            Resources.LoadAll<T>(path);
    }
}