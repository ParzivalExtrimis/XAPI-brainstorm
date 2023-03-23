using Pancake.Models;
using Unity.Networking;
using System;

namespace Pancake {
    
    public class XAPI {

        private const string URL = "backend.xapi.windows.net/api/Sender";
        public void Trigger(Pancake.Models.User actor, string verb, string obj) {
            Trigger(actor, verb, obj, "Success");
        }

        public void Trigger(Pancake.Models.User actor, string verb, string obj, string result) {
            var json = GetPayLoad(actor, verb, obj, result);
            SendRequest(URL, json);
        }    

        private string GetPayLoad(Pancake.Models.User actor, string verb, string obj, string result) {
            var id = new Guid().ToString();
            var time = UTC.DateTime.Now.ToString();
            return $@"
            {{
                ""id"": {id},
                ""user"": {actor.UserID},
                ""statement"":
                    {{
                        ""actor"" : {{
                            ""name"" : ""{actor.Name}"",
                            ""mbox"" : ""mailto:{actor.Email}"",
                            ""ObjectType"" : ""agent""
                        }},
                        ""verb"" : {{
                            ""id"" : ""https://example.com/actions/touch"", -> touch
                            ""display"" : {{
                                ""en-US"" : ""{verb}""
                            }}
                        }},
                        ""object"" : {{
                            ""id"" : ""https://example.com/articles/containers/bottle"", -> bottle
                            ""definition"" : {{
                                ""name"" : {{
                                    ""en-US"" : ""{obj}""
                                }}
                            }}
                        }}
                        ""timestamp"": {time}
                    }}
                }}
                ";
        }

        IEnumerator SendRequest(string url, string jsonData) {
            UnityWebRequest request = UnityWebRequest.Post(url, jsonData);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                Debug.Log("POST request successful!");
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else {
                Debug.LogError("POST request failed: " + request.error);
            }
        }
    }

}