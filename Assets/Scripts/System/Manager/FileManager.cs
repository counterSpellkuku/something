using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.PlayerSave;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace System.Manager
{
    public class FileManager
    {
        private static string savePath
        {
            get
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string gamePath = Path.Combine(appData, Application.productName);
            
                if (!Directory.Exists(gamePath))
                    Directory.CreateDirectory(gamePath);
                
                return gamePath;
            }
        }

        
        
        public static void SaveData(string fileName = "PlayerShadow", params PlayerState[] data) {
            string filePath = Path.Combine(savePath, $"{fileName}.json");
            StringBuilder builder = new StringBuilder();
            bool error = false;
            builder.AppendLine("{");
            
            
            foreach (PlayerState state in data)
            {
                string jsonData = ToJson(state);
                builder.Append(jsonData);
                builder.Append(",\n");
            }
            builder.AppendLine("}");

            try {
                File.WriteAllText(filePath, builder.ToString());
                Debug.Log($"Data saved successfully to: {filePath}");
            }
            catch (Exception e)
            {
                error = true;
                Debug.LogError($"Failed to save data: {e.Message}");
            }
        }

        public static List<PlayerState> GetShadow(string name = "/PlayerShadow.json")
        {
            string filePath = savePath + name;
            try {
                
                if (!File.Exists(filePath)) {
                    Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
                    return null;
                }

                string jsonContent = File.ReadAllText(filePath);

                List<PlayerState> gameDataList = GetPlayerShadow(jsonContent);
                Debug.Log($"성공적으로 {gameDataList.Count.ToString()}개의 GameData를 로드했습니다.");
                return gameDataList;
            }
            catch (Exception e)
            {
                Debug.LogError($"파일 로드 중 오류 발생: {e.Message}");
                return null;
            }
        }
        private static List<PlayerState> GetPlayerShadow(string jsonString) {
            List<PlayerState> results = new List<PlayerState>();

            try {
                // 중괄호로 둘러싸인 json을 찾습니다 제발 찾습니다. 제발 좀요
                string pattern = @"{[\s\S]*?position[\s\S]*?activeKeys[\s\S]*?}";
                MatchCollection matches = Regex.Matches(jsonString, pattern);

                foreach (Match match in matches) {
                    PlayerState parsedData = ParseSingleGameData(match.Value);
                    if (parsedData != null)
                        results.Add(parsedData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"error: {e.Message}");
            }

            return results;
        }

        private static PlayerState ParseSingleGameData(string jsonString) {
            try {
                PlayerState data = new PlayerState();
                data.activeKeys = new HashSet<KeyCode>();

                // position
                Match posMatch = Regex.Match(jsonString, @"""position"":\s*{\s*""x"":\s*\(([-\d.]+),\s*([-\d.]+)\),\s*""y"":\s*\(([-\d.]+),\s*([-\d.]+)\)}");
                if (posMatch.Success)
                {
                    float x1 = float.Parse(posMatch.Groups[1].Value);
                    float x2 = float.Parse(posMatch.Groups[2].Value);
                    float y1 = float.Parse(posMatch.Groups[3].Value);
                    float y2 = float.Parse(posMatch.Groups[4].Value);
                    data.position = new Vector2(x1, y1); // 첫 번째 값만 사용하거나 필요에 따라 수정
                }

                // velocity
                Match velMatch = Regex.Match(jsonString, @"""currentVelocity"":\s*{\s*""x"":\s*\(([-\d.]+),\s*([-\d.]+)\),\s*""y"":\s*\(([-\d.]+),\s*([-\d.]+)\)}");
                if (velMatch.Success)
                {
                    float x1 = float.Parse(velMatch.Groups[1].Value);
                    float x2 = float.Parse(velMatch.Groups[2].Value);
                    float y1 = float.Parse(velMatch.Groups[3].Value);
                    float y2 = float.Parse(velMatch.Groups[4].Value);
                    data.currentVelocity = new Vector2(x1, y1); // 첫 번째 값만 사용하거나 필요에 따라 수정
                }

                // timestamp
                Match timeMatch = Regex.Match(jsonString, @"""timestamp"":\s*([-\d.]+)");
                if (timeMatch.Success)
                {
                    data.timestamp = float.Parse(timeMatch.Groups[1].Value);
                }

                // deltaTime
                Match deltaMatch = Regex.Match(jsonString, @"""deltaTime"":\s*([-\d.]+)");
                if (deltaMatch.Success)
                {
                    data.deltaTime = float.Parse(deltaMatch.Groups[1].Value);
                }

                // activeKeys
                Match keysMatch = Regex.Match(jsonString, @"""activeKeys"":\s*\[([\s\d,]*)\]");
                if (keysMatch.Success)
                {
                    string keysStr = keysMatch.Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(keysStr))
                    {
                        string[] keyArray = keysStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string keyStr in keyArray)
                        {
                            if (int.TryParse(keyStr.Trim(), out int keyCode))
                            {
                                data.activeKeys.Add((KeyCode)keyCode);
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Parse Error: {e.Message}");
                return null;
            }
        }

        private static string ToJson(PlayerState data) {
            if (data == null) return "";

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.AppendLine("{");
        
            
            jsonBuilder.AppendLine($"  \"position\": {{");
            jsonBuilder.AppendLine($"    \"x\": {data.position.ToString()},");
            jsonBuilder.AppendLine($"    \"y\": {data.position.ToString()}");
            jsonBuilder.AppendLine("  },");
        
            
            jsonBuilder.AppendLine($"  \"currentVelocity\": {{");
            jsonBuilder.AppendLine($"    \"x\": {data.currentVelocity.ToString()},");
            jsonBuilder.AppendLine($"    \"y\": {data.currentVelocity.ToString()}");
            jsonBuilder.AppendLine("  },");
        
            
            jsonBuilder.AppendLine($"  \"timestamp\": {data.timestamp.ToString()},");
            jsonBuilder.AppendLine($"  \"deltaTime\": {data.deltaTime.ToString()},");
        
            
            jsonBuilder.AppendLine("  \"activeKeys\": [");
            string keysList = string.Join(",\n    ", 
                data.activeKeys.Select(key => $"\"{((int)key).ToString()}\""));
            jsonBuilder.AppendLine($"    {keysList}");
            jsonBuilder.AppendLine("  ]");
        
            jsonBuilder.Append("}");
        
            return jsonBuilder.ToString();

            
        } 
    }
    
    
}