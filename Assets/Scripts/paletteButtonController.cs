using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class paletteButtonController : MonoBehaviour
{
	[SerializeField] private GameObject palleteButton;
	[SerializeField] private InputField findInput;
	public void FindAll ()
	{
		Color cloure = new Color ();
		ButtonManager.Instance.buttonList.ForEach (button => Destroy (button));
		Color palleteColor = gameObject.GetComponent<Image> ().color;
		string findcolor = ColorUtility.ToHtmlStringRGB (palleteColor);
		bool trey = ColorUtility.TryParseHtmlString ("#" + findcolor, out cloure);
		for (int i = 0; i < RecordsDataBase.Marks.Count; i++)
		{
			if (findInput.text == "")
			{
				if (findcolor == (RecordsDataBase.Marks[i]))
				{
					ButtonManager.Instance.CreateButton (i, RecordsDataBase.Records[i], RecordsDataBase.Marks[i]);
					ButtonManager.Instance.SetContentParameters ();
				}
				else
				{
					Debug.Log ("THERE IS NO SUCH MARKS");
				}
			}
			else if (findcolor == (RecordsDataBase.Marks[i]) && RecordsDataBase.Records[i].Contains (findInput.text))
			{
				ButtonManager.Instance.CreateButton (i, RecordsDataBase.Records[i], RecordsDataBase.Marks[i]);
				ButtonManager.Instance.SetContentParameters ();
			}

			else if (findcolor == (RecordsDataBase.Marks[i]) && findInput.text == "")
			{
				ButtonManager.Instance.CreateButton (i, RecordsDataBase.Records[i], RecordsDataBase.Marks[i]);
				ButtonManager.Instance.SetContentParameters ();
			}
		}
	}
}