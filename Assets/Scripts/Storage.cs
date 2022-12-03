using System.IO;
using Newtonsoft.Json;

public static class Storage
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.All
    };

    public static string SerializeObject<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, Settings);
    }

    public static T DeserializeObject<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str, Settings);
    }
    
    public static void Save<T>(string filePath, T records)
    {
        var j = SerializeObject(records);
        File.WriteAllText(filePath, j);
    }

    public static T Load<T>(string filePath) where T : new()
    {
        if (!File.Exists(filePath))
            return new T();
        
        var str = File.ReadAllText(filePath);
        return DeserializeObject<T>(str);
    }
}