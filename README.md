LitMiniJson
===================================
  Re-encapsulate LitJson for supporting iOS. Replace the module of parsing with MiniJson.

### Features:
  1. The Same API with LitJson.<br />
  2. Support iOS/Android/WP8/Win8.<br />
  3. Stable and good performance.<br />

### Sample:
    /*
    There is a string named 'str' with contents as below:
    {
        "status":0,
        "msg":"ok",
        "data":
        {
            "friends":
            [
                {"id":1, "name":"night"},
                {"id":3, "name":"tom"}
            ]
        }
    }
    */

    using LitJson;

    // string --> json object
    JsonData contentData = JsonMapper.ToObject(str);
    JsonData friendsData = contentData["data"]["friends"];
    for (int i = 0; i < friendsData.Count; i++) {
        int id = int.Parse(friendsData[i]["id"].ToString()); // 1 or 3
        string name = friendsData[i]["name"].ToString();     // night or tom
    }

    // json object --> string
    string strFriends = friendsData.ToJson(); // strFriends is "[{\"id\":1, \"name\":\"night\"},{\"id\":3, \"name\":\"tom\"}]"

    // create or modify json object
    JsonData newFriend = new JsonData();
    newFriend["id"] = 4;
    newFriend["name"] = "terry";
    friendsData.Add(newFriend);
    strFriends = friendsData.ToJson(); // strFriends is "[{\"id\":1, \"name\":\"night\"},{\"id\":3, \"name\":\"tom\"},{\"id\":4, \"name\":\"terry\"}]"


中文说明:
-----------------------------------
LitJson因为使用了反射，不支持iOS平台。所以我把解析模块替换成了MiniJson。

### 特性：
  1. 接口跟LitJson完全一致。<br />
  2. 支持iOS/Android/WP8/Win8等平台。<br />
  3. 线上多款游戏长期稳定运行，可靠性和性能完全没问题。

P.S. 如果对你有帮助，请点右上角的Star表示支持，谢谢！
