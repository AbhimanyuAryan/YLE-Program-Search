using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Class to manage insertion to and deletion from the Scroll rect (UI list) 
public class AppendProgramList : MonoBehaviour {

    public Image panel;

    private int programsShownCount = 0;

    //Add one new entity to Scroll rect
    public void AppendProgram(string name)
    {
        Image copy = Instantiate(panel);
        copy.GetComponentInChildren<Text>().text = name;
        copy.transform.SetParent(transform);
        copy.enabled = true;
        ++programsShownCount;
    }

    //Clear Scroll rect
    public void ClearList()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        programsShownCount = 0;
    }

    //Returns count of entities shown currently
    public int GetShownCount()
    {
        return programsShownCount;
    }
}
