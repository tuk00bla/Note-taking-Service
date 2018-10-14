using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{

	[SerializeField] private GameObject page2;
	[SerializeField] private GameObject page1;
	[SerializeField] private InputField inputNote;
	[SerializeField] private InputField findinput;
	[SerializeField] private GameObject palettePanel;
	[SerializeField] private GameObject noteContent;
	[SerializeField] private GameObject buttonPrefab;
	public GameObject currOpenedButton;
	private int buttonCount;

	public static ButtonManager Instance;

	public GameObject centerPanel;

	public List<GameObject> buttonList;

	public Color standartColor = new Color (255, 255, 255, 100);

	public int indx;
	public int buttonIndex { get; set; }

	private void Awake ()
	{
		Instance = this;
	}
	private void Start ()
	{
		palettePanel.transform.SetParent (noteContent.transform);
		palettePanel.transform.SetSiblingIndex (0);
		palettePanel.SetActive (false);
		RecordsDataBase.UpdateRecordsFromFile ();
		List<GameObject> buttonList = new List<GameObject> (RecordsDataBase.Records.Count);
		GenerateRecordDisplay ();
	}

	public void NewText ()
	{
		inputNote.text = "";

		page2.SetActive (true);
		page1.SetActive (false);
		palettePanel.SetActive (false);
	}

	public void BackToMain ()
	{
		inputNote.text = "";
		findinput.text = "";
		inputNote.GetComponent<Image> ().color = standartColor;

		UpdateRecordDisplay (buttonList);

		page2.SetActive (false);
		page1.SetActive (true);
		palettePanel.SetActive (false);
	}
	private void OnApplicationFocus (bool focusStatus)
	{
		if (!focusStatus)
		{
			RecordsDataBase.RefreshFile ();
			Debug.Log ("loseFocus");
		}
	}

	private void OnApplicationQuit ()
	{
		RecordsDataBase.RefreshFile ();
		Debug.Log ("Quit");
	}

	public void GenerateRecordDisplay ()
	{
		if (RecordsDataBase.Records != null && RecordsDataBase.Records.Count > 0)
		{
			for (int i = 0; i < RecordsDataBase.Records.Count; i++)
			{
				CreateButton (i, RecordsDataBase.Records[i], RecordsDataBase.Marks[i]);
			}
		}
	}

	public void UpdateRecordDisplay (List<GameObject> list)
	{
		list.ForEach (button => Destroy (button));
		GenerateRecordDisplay ();
	}

	public void CreateButton (int buttonIndex, string buttonContent, string createClr) // ДЛЯ СОЗДАНИЯ КНОПОК, ИНФА БЕРЕТСЯ ИЗ ЛИСТОВ
	{
		Color convertedColor = new Color ();
		bool isConverted = ColorUtility.TryParseHtmlString ("#" + createClr, out convertedColor);
		GameObject button = GameObject.Instantiate (buttonPrefab);

		button.name = buttonIndex.ToString ();

		button.GetComponent<Test_NoteButtonController> ().buttonIndx = buttonIndex;
		buttonCount = buttonIndex;
		button.GetComponentInChildren<Text> ().text = buttonContent;
		button.GetComponent<Image> ().color = convertedColor;

		button.transform.SetParent (noteContent.transform);
		button.transform.position = noteContent.transform.position;
		button.transform.localScale = new Vector3 (1, 1, 1);

		buttonList.Add (button);
		Debug.Log ("TY PETUH " + buttonCount);
		SetContentParameters (buttonCount + 1);
	}

	public void OpenNote (GameObject button)
	{
		Color openedColor = new Color ();
		string openedColorString = RecordsDataBase.Marks[button.GetComponent<Test_NoteButtonController> ().buttonIndx];
		bool colorIsConverted = ColorUtility.TryParseHtmlString ("#" + openedColorString, out openedColor);
		Debug.Log ("KEKNYT: " + colorIsConverted);
		inputNote.text = RecordsDataBase.Records[button.GetComponent<Test_NoteButtonController> ().buttonIndx];
		inputNote.GetComponent<Image> ().color = openedColor;

		page1.SetActive (false);
		page2.SetActive (true);
	}

	public void EditNote ()
	{
		if (currOpenedButton == null) return;
		int editInd = currOpenedButton.GetComponent<Test_NoteButtonController> ().buttonIndx;
		string editText = inputNote.text;
		Color inputColor = inputNote.GetComponent<Image> ().color;
		RecordsDataBase.EditRecord (editInd, editText, inputColor);
		BackToMain ();
	}

	public void DeleteNote ()
	{
		// Если параметры не переданы => удаляем "сохраненную" кнопку
		if (currOpenedButton == null) return;
		int deleteInd = currOpenedButton.GetComponent<Test_NoteButtonController> ().buttonIndx;
		RecordsDataBase.RemoveAt (deleteInd);
		Destroy (currOpenedButton);
		BackToMain ();
	}

	public void DeleteNote (GameObject deleteButton)
	{
		int deleteInd = deleteButton.GetComponent<Test_NoteButtonController> ().buttonIndx;
		RecordsDataBase.RemoveAt (deleteInd);
		Destroy (deleteButton);
	}

	public void SaveNote (string newText)
	{
		newText = inputNote.text;
		int saveInd = (buttonList.Count + 1);
		string[] lines = newText.Split ('\n');
		string caption = lines[0].ToString ();
		int newLineInd = lines[0].ToString ().Length;
		string content = newText.Substring (newLineInd);

		Debug.Log (caption);
		RecordsDataBase.AddNewRecord (ColorUtility.ToHtmlStringRGB (inputNote.GetComponent<Image> ().color), caption, content);
		BackToMain ();
	}

	public void FindNote (string findText)
	{
		palettePanel.SetActive (false);
		buttonList.ForEach (button => Destroy (button));
		findText = findinput.text;
		if (findText == "")
		{
			Debug.Log ("VVEDITE TEXT");
			GenerateRecordDisplay ();

		}
		else
		{
			for (int i = 0; i < RecordsDataBase.Records.Count; i++)
			{
				if (RecordsDataBase.Records[i].Contains (findText))
				{
					string foundItem = RecordsDataBase.Records[i];
					Debug.Log ("SUKA TI ETO ISKAL?");
					Debug.Log ("Na, derzhi <" + foundItem + ">");
					CreateButton (RecordsDataBase.Records.IndexOf (foundItem), foundItem, RecordsDataBase.Marks[i]);
				}
			}
		}
	}

	public void SetContentParameters ()
	{
		int panelCount = noteContent.transform.childCount;
		float spacing = noteContent.GetComponent<GridLayoutGroup> ().spacing.y;
		float contentHeight = noteContent.GetComponent<RectTransform> ().sizeDelta.y;
		float panelHeight = noteContent.GetComponent<GridLayoutGroup> ().cellSize.y;

		float contentPaddingTop = noteContent.GetComponent<GridLayoutGroup> ().padding.top;
		float contentPaddingBottom = noteContent.GetComponent<GridLayoutGroup> ().padding.bottom;

		float startContentHeight = panelHeight * panelCount + spacing * (panelCount - 1) + contentPaddingTop + contentPaddingBottom;
		float startContentPosition = -((startContentHeight - contentHeight) / 2);

		noteContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (noteContent.GetComponent<RectTransform> ().sizeDelta.x, startContentHeight);
		noteContent.GetComponent<RectTransform> ().position = new Vector3 (noteContent.GetComponent<RectTransform> ().position.x, startContentPosition, noteContent.GetComponent<RectTransform> ().position.z);
	}

	public void SetContentParameters (int panelCount)
	{
		float spacing = noteContent.GetComponent<GridLayoutGroup> ().spacing.y;
		float contentHeight = noteContent.GetComponent<RectTransform> ().sizeDelta.y;
		float panelHeight = noteContent.GetComponent<GridLayoutGroup> ().cellSize.y;

		float contentPaddingTop = noteContent.GetComponent<GridLayoutGroup> ().padding.top;
		float contentPaddingBottom = noteContent.GetComponent<GridLayoutGroup> ().padding.bottom;

		float startContentHeight = panelHeight * panelCount + spacing * (panelCount - 1) + contentPaddingTop + contentPaddingBottom;
		float startContentPosition = -((startContentHeight - contentHeight) / 2);

		noteContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (noteContent.GetComponent<RectTransform> ().sizeDelta.x, startContentHeight);
		noteContent.GetComponent<RectTransform> ().position = new Vector3 (noteContent.GetComponent<RectTransform> ().position.x, startContentPosition, noteContent.GetComponent<RectTransform> ().position.z);
	}

	public void OpenPallete ()
	{
		palettePanel.SetActive (true);
	}
}