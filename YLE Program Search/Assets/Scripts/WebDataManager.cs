using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;

public class WebDataManager : MonoBehaviour {

	public ScrollRect view;         //UI list
    public Button submitButton;     //Submit button
    public InputField input;        //Input field
    public Scrollbar scroll;        //Scroll bar next to UI list

	private List<Title> programList;
	JsonData playerJson;

	void Start() {
		// Add Listeners for submit button is pressed
		submitButton.onClick.AddListener(delegate { handleInput(); });
		//when the scrollbar value changes 
		scroll.onValueChanged.AddListener(delegate { updateView(); });

		input.onEndEdit.AddListener(delegate { handleInput(); });

		programList = new List<Title>();
	}

	// Starts routine to get data from API
	void handleInput() {
		if(input.text != "") {
			processQuery(input.text);
			input.text = "";
		}
	}

	// Coroutine for data fetch
	void processQuery(string keyword) {
		StartCoroutine(getDataFromAPI(keyword));
	}

	IEnumerator getDataFromAPI(string searchKeyword) {
		string url = "https://external.api.yle.fi/v1/programs/items.json?app_id=6e95a336&app_key=420f34d7cf92076ee50c905a07b4ee9d&limit=100&availability=ondemand&mediaobject=video&q=" + searchKeyword;
		WWW wwwObj = new WWW(url);

		yield return wwwObj;

		// check for errors
		if(wwwObj.error == null)
		{
			Debug.Log("WWW Ok!: " + wwwObj.text);
			ProcessJson(wwwObj.text);
            updateView();
		} else {
			Debug.Log("WWW Error: " + wwwObj.error);
		}
	}

	void ProcessJson(string jsonString) {
		JsonReader reader = new JsonReader(jsonString);
        JsonData data = JsonMapper.ToObject(reader);
        ClearCache();

		try
        {
            List<Title> duplicatesList = new List<Title>();
            for (int i = 0; i < data["data"].Count; ++i)
            {
                //Make sure at least one title is found (either fi or sv)
                //Both titles are found
                if (data["data"][i]["title"].Keys.Contains("sv") && data["data"][i]["title"].Keys.Contains("fi"))
                {
                    duplicatesList.Add(new Title(data["data"][i]["title"]["fi"].ToString(),
                                                data["data"][i]["title"]["sv"].ToString()));
                }
                //Only fi title is found
                else if (data["data"][i]["title"].Keys.Contains("fi"))
                {
                    duplicatesList.Add(new Title(data["data"][i]["title"]["fi"].ToString(), ""));
                }
                //Only sv title is found
                else if (data["data"][i]["title"].Keys.Contains("sv"))
                {
                    duplicatesList.Add(new Title("", data["data"][i]["title"]["sv"].ToString()));
                }
                else
                {
                    Debug.Log("Error: Entity with no title");
                }
                
            }
            //Get rid of duplicate titles
            programList = duplicatesList.Distinct().ToList();
        }
        catch (Exception e)
        {
            Debug.Log("Exception caught: " + e.Message);
        }
	}

	    //Pushes more entities into Scroll rect (UI list)
    void updateView()
    {
        //Used to handle unnecessary listener calls from scroll bar
        if (scroll.value != 0f) return; 

        //Push 10 more entities into Scroll rect
        int countBefore = view.GetComponentInChildren<AppendProgramList>().GetShownCount();
        for (int i = countBefore; i < programList.Count; ++i)
        {
            if (i == countBefore + 10) break;
            //Prefer finnish title but if missing, use swedish
            string title = (programList.ElementAt(i).fi == "" ? programList.ElementAt(i).sv : programList.ElementAt(i).fi);
            view.GetComponentInChildren<AppendProgramList>().AppendProgram(title);
        }

        //Update scroll value to a position where the list doesn't change when new entities are added
        int countAfter = view.GetComponentInChildren<AppendProgramList>().GetShownCount();
        if (countAfter > 10)
        {
            scroll.value = 1 / (countAfter / 10);
        }
        
    }

	void ClearCache()
    {
        view.GetComponentInChildren<AppendProgramList>().ClearList();
        programList.Clear();
    }
}

//Class that holds the title data
class Title : IEquatable<Title>
{
    public Title(string fiName, string svName)
    {
        fi = fiName;
        sv = svName;
    }

    //Implemented IEquatable interface so that Distinct() can pick out duplicates away
    public override int GetHashCode()
    {
        int hashProductFi = fi.GetHashCode();
        int hashProductSv = sv.GetHashCode();
        return hashProductFi ^ hashProductSv;
    }

    //Implemented IEquatable interface so that Distinct() can pick out duplicates away
    public bool Equals(Title other)
    {
        if (other == null)
            return false;

        if (fi == other.fi && sv == other.sv)
            return true;
        else
            return false;
    }

    //Payload
    public string fi { get; set; }
    public string sv { get; set; }
}