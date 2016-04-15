using UnityEngine;
using System.Xml;

public class Tested : MonoBehaviour
{
    public string   dialogueURL;
    //public TextAsset dialogueURL;
    public int      index;
    public string   lastSearch;
    public string   nextSearch;

    void Start()
    {
        XmlReader reader = XmlReader.Create(dialogueURL);
        while (reader.Read())
        {
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == nextSearch))
            {
                lastSearch = nextSearch;
                if (reader.HasAttributes)
                {
                    print(reader.GetAttribute("answer"));
                }
                else
                {
                    //Quit
                }
            }
        }
    }

    void Rec(int i)
    {
        nextSearch = lastSearch + "_" + i;
    }
}