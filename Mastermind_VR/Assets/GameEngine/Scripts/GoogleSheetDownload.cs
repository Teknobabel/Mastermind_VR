using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public class GoogleSheetDownload
{
    private const string spreadSheetId = "1YOR4f41GPTI1rIn2iehRbeanI4JTdGKP9-pwfRGNtsM";
    public static IEnumerator GetEventData (MonoBehaviour gameManager) {
        Action<string> commCallback = (csv) =>
                {
                   List<List<string>> eventData = ParseCSV(csv);
                   ((GameManager)gameManager).m_eventData = eventData;
                };
        yield return gameManager.StartCoroutine(DownloadCSVCoroutine(spreadSheetId, commCallback, true, "YOUR_ASSET_NAME_NO_EXT", null));
    }
//    YOUR METHOD HERE - BEGIN

// if (SOME_CONTROL_OF_YOURS && Application.isEditor)
//             {
//                 Action<string> commCallback = (csv) =>
//                 {
//                     LoadCSVText(csv);
//                 };
//                 StartCoroutine(DownloadCSVCoroutine(spreadSheetId, commCallback, true, "YOUR_ASSET_NAME_NO_EXT", tabId));
//             }
//             else
//             {
//                 var data = Resources.Load("YOUR_ASSET_NAME_NO_EXT") as TextAsset;
//                 LoadCSVText(data.text);
//             }

// YOUR METHOD HERE - END

public static List<List<string>> ParseCSV(string text)
        {
            text = CleanReturnInCsvTexts(text);

            var list = new List<List<string>>();
            var lines = Regex.Split(text, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);

            bool jumpedFirst = false;

            foreach (var line in lines)
            {
                if (!jumpedFirst)
                {
                    jumpedFirst = true;
                    continue;
                }
                var values = Regex.Split(line, SPLIT_RE);

                var entry = new List<string>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    var value = values[j];
                    value = DecodeSpecialCharsFromCSV(value);
                    entry.Add(value);
                    //Debug.Log("value: " + value);
                }
                list.Add(entry);
                //Debug.Log("entry: " + entry);
            }
            //BuildEvents(list);
            
            return list;
        }

public static string CleanReturnInCsvTexts(string text)
        {
            text = text.Replace("\"\"", "'");

            if (text.IndexOf("\"") > -1)
            {
                string clean = "";
                bool insideQuote = false;
                for (int j = 0; j < text.Length; j++)
                {
                    if (!insideQuote && text[j] == '\"')
                    {
                        insideQuote = true;
                    }
                    else if (insideQuote && text[j] == '\"')
                    {
                        insideQuote = false;
                    }
                    else if (insideQuote)
                    {
                        if (text[j] == '\n')
                            clean += "<br>";
                        else if (text[j] == ',')
                            clean += "<c>";
                        else
                            clean += text[j];
                    }
                    else
                    {
                        clean += text[j];
                    }
                }
                text = clean;
            }
            return text;
        }

public static IEnumerator DownloadCSVCoroutine(string docId, Action<string> callback, 
                                               bool saveAsset = false, string assetName = null, string sheetId = null)
        {
            string url =
                "https://docs.google.com/spreadsheets/d/" + docId + "/export?format=csv";

            if (!string.IsNullOrEmpty(sheetId))
                url += "&gid=" + sheetId;

            WWWForm form = new WWWForm();
            WWW download = new WWW(url, form);

            yield return download;
            Debug.Log(download.text);
            if (!string.IsNullOrEmpty(download.error))
            {
                Debug.Log("Error downloading: " + download.error);
            }
            else
            {
                callback(download.text);
                // if (saveAsset)
                // {
                //     if (!string.IsNullOrEmpty(assetName))
                //         File.WriteAllText("Assets/Resources/" + assetName + ".csv", download.text);
                //     else
                //     {
                //         throw new Exception("assetName is null");
                //     }
                // }
            }
        }

        //CSV reader from https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/

        public static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        public static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        public static readonly char[] TRIM_CHARS = { '\"' };

        public static List<List<string>> ReadCSV(string file)
        {
            var data = Resources.Load(file) as TextAsset;
            return ParseCSV(data.text);
        }

        public static string DecodeSpecialCharsFromCSV(string value)
        {
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Replace("<br>", "\n").Replace("<c>", ",");
            return value;
        }
}
