using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public static class RecordsDataBase
{
	public static List<string> Records;
	public static List<string> Marks;
	public static void UpdateRecordsFromFile ()
	{
		// string filePathInResources = "Notes";
		// string fileStringData = Resources.Load<TextAsset> (filePathInResources).text;
		Records = new List<string> ();
		Marks = new List<string> ();
		string path = Application.persistentDataPath + "Notes.json";
		string jsonStringData = string.Empty;

		if (File.Exists (path))
		{
			jsonStringData = File.ReadAllText (path);
			JSONObject newObj = new JSONObject (jsonStringData);
			for (int i = 0; i < newObj.Count; i++)
			{
				string[] separatingChars = { "#neXtIsColor228#" };
				string[] objects = newObj[i].str.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
				Records.Add (objects[0]);
				Marks.Add (objects[1]);
			}
		}

	}

	public static void AddNewRecord (string clr, string content)
	{

		if (!String.IsNullOrEmpty (content))
		{
			Records.Add (content);
			Marks.Add (clr);
		}
		else Debug.Log ("HYLE TY NE PISHESH");
	}
	public static void AddNewRecord (string clr, string caption, string content)
	{
		if (!String.IsNullOrEmpty (caption))
		{
			if (!String.IsNullOrEmpty (content))
			{
				Records.Add (caption + content);
				Marks.Add (clr);
			}
			if (String.IsNullOrEmpty (content))
			{
				Records.Add (caption);
				Marks.Add (clr);
			}
		}
		if (String.IsNullOrEmpty (caption))
		{
			if (!String.IsNullOrEmpty (content))
			{
				string captionDate = DateTime.Today.ToString ("dd/MM/yyyy");
				Records.Add (captionDate + content);
				Marks.Add (clr);
			}
			else
			{
				ButtonManager.Instance.BackToMain ();
			}
		}
	}
	public static void EditRecord (int index, string content, Color clr)
	{
		string colorContent = ColorUtility.ToHtmlStringRGB (clr);

		Records.Remove (Records[index]);
		Records.Insert (index, content);

		Marks.Remove (Marks[index]);
		Marks.Insert (index, colorContent);
	}
	public static void RemoveAt (int index)
	{
		Records.RemoveAt (index);
		Marks.RemoveAt (index);
	}
	public static void RefreshFile ()
	{
		JSONObject newObj = new JSONObject ();
		string path = Application.persistentDataPath + "Notes.json";
		if (Records.Count > 0 && Marks.Count > 0)
		{

			for (int i = 0; i < Records.Count; i++)
			{
				newObj.AddField (i.ToString (), Records[i] + "#neXtIsColor228#" + Marks[i]);
			}

			File.WriteAllText (path, newObj.ToString ());
		}
		if (Records.Count == 0)
		{
			Debug.Log ("NE ZAPISAL");
			string emptyField = "{}";
			File.WriteAllText (path, emptyField);
		}
		if (Records.Count < 0)
		{
			Debug.Log ("Something is wrong");
		}

	}
}