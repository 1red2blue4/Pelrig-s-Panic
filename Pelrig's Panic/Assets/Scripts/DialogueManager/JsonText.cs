using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;

namespace JSONText
{
    class JSONAssembly
    {
        //Add the json scripts here for the cut scenes.
        private static Dictionary<int, string> resourceList = new Dictionary<int, string>
        {
        {1, "/Resources/Cutscene1.json"},
        {2, "/Resources/Cutscene2.json"}
        };
        public static NarrativeEvent RunJSONFactoryForScene(int sceneNumber)
        {
            string resourcePath = PathForScene(sceneNumber);

            if(IsValidJSON(resourcePath) == true)
            {
                string jsonString = File.ReadAllText(Application.streamingAssetsPath + resourcePath);
                NarrativeEvent narrativeEvent = JsonMapper.ToObject<NarrativeEvent>(jsonString);

                return narrativeEvent;
            }
           else
            {
                throw new Exception("The scene number not in the list");
            }
        }

        private static string PathForScene(int sceneNumber)
        {
            string resourcePathResult;
            if(resourceList.TryGetValue(sceneNumber, out resourcePathResult))
            {
                return resourceList[sceneNumber];
            }
            else
            {
                throw new Exception("The scene number not in the list");
            }
        }
        private static bool IsValidJSON(string path)
        {
            return (Path.GetExtension(path) == ".json") ? true : false;
        }

    }
}

