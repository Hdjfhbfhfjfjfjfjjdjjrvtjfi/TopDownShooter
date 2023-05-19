using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

public class HTTPRequest : Godot.HTTPRequest
{
    [Export]
    private string Uri { get; set; }
    [Signal]
    public delegate void ReturnData(int[] highScores);
    public void GetData()
    {
        Request(url: Uri+"get", method: HTTPClient.Method.Get);
    }
    public void SetData(int score)
    {
        Request(url: Uri + "set" + $"/{score}", method: HTTPClient.Method.Get);
    }
    public void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
        try
        {
            if (json.Result.ToString().Replace("[", "").Replace("]", "").ToInt() == 1)
            {
                GD.Print("Data Saved");
            }
        }
        catch
        {
            string[] stringScores = json.Result.ToString().Replace("[", "").Replace("]", "").Split(", ");
            int[] scores = new int[stringScores.Length];
            for (int i = 0;i< stringScores.Length; i++)
            {
                scores[i] = stringScores[i].ToInt();
            }
            EmitSignal(nameof(ReturnData), new object[1] { scores });
        }
    }
}
