using System.Text.Json;


namespace FinCli.Core;

public class TransactionStorage
{
    public void Save(List<Transaction> transactions, string path)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string SerializedJson = JsonSerializer.Serialize(transactions, options);
        File.WriteAllText(path, SerializedJson);

    }

    public List<Transaction> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new List<Transaction>();
        }

        string Json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Transaction>>(Json) ?? new List<Transaction>();
    }
}

