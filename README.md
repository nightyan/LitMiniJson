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
    int count = friendsData.Count;
    for (int i = 0; i < count; i++) {
        int id = int.Parse(friendsData[i]["id"].ToString()); // 1 or 3
        string name = friendsData[i]["name"].ToString();     // night or tom
    }
    
    // foreach形式，必须显式指明JsonData类型
    foreach (JsonData data in friendsData) { // 此处不可写var data
        int id = int.Parse(data.ToString());   // 1 or 3
        string name = data["name"].ToString(); // night or tom
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
LitJson因为JsonMapper使用了iOS不支持的反射特性，不能在iOS上使用。所以我参照LitJson的接口，解析模块用MiniJson，封装出了LitMiniJson。

### 特性：
  1. 接口跟LitJson完全一致，可以直接替换LitJson。<br />
  2. 跨平台：支持iOS/Android/WP8/Win8/Win32/Mac等平台，其它平台未测试。<br />
  3. 线上多款游戏长期稳定运行，可靠、良好的性能。

P.S. 如果对你有帮助，请点右上角的Star表示支持，谢谢！
