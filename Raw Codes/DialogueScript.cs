using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;

public class DialogueScript : MonoBehaviour
{
	public string xmlFile, lastSearch, nextSearch;//in xmlFile zet je de naam van de file in dit geval dia.xml en in nextSearch zet je o (van optie) 
	private int chatboxCount;

	public GameObject buttonPref; //prefab van de button (button mag geen width of Height hebben)
	public RectTransform chatContainer; //deze word gevult met buttons
	public RectTransform response;//box waar de response van de NPC op weer word gegeven

	List<GameObject> boxes = new List<GameObject>();
	List<string> putInBoxes = new List<string>();

	private RectTransform lastPos;

	void Start() //Start met het readen van de xmlFile (self explainatory)
	{
		Read(); 
	}

	void Read() //deze void haalt alle info uit de xml file
	{
		XmlReader reader = XmlReader.Create(Application.dataPath + "/" + xmlFile); //dit creeerd een XmlReader met de url van de xmlfolder op de pc die het gebruikt + de naam van de xmlFile +.xml erachter

		while (reader.Read()) //als hij read zoekt hij naar het element wat opgeslagen staat als nextsearch.
		{
			if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == nextSearch))
			{
				lastSearch = nextSearch; //zet de search die net is gebruikt als lastsearch zodat t script dit kan gebruiken als reverentie punt om de boel back te tracen.
				if (reader.HasAttributes)
				{
                    if (int.Parse(reader.GetAttribute("buttoncount")) != 0)
                    {
						chatboxCount = int.Parse(reader.GetAttribute("buttoncount"));
						response.GetComponent<Text>().text = reader.GetAttribute("answer");

						for(int i = 1; i <= chatboxCount; i++)
						{
							reader.Read();	//read opnieuw en nu totdat hij bij de lastSearch+het huidige getal is en propt zijn textattribute in een list
							if (reader.Name == lastSearch+"_"+i)
							{
								putInBoxes.Add(reader.GetAttribute("text"));
							}
						}
                    }
                    else
                    {
                        print("QUIT");

                    }
				}
			}
		}
		GenerateChatBoxes();
	}

	void GenerateChatBoxes()
	{
		float sizeY = (chatContainer.sizeDelta.y / chatboxCount) / (chatContainer.localScale.y / chatContainer.lossyScale.y); //deelt t aantal boxes door de grootte van de container waar de boxes in moeten.

		for (int i = 1; i <= chatboxCount; i++) //zolang er nog boxes zijn om de container te vullen herhaalt hij \/
		{

			GameObject box = Instantiate(buttonPref, new Vector3(chatContainer.position.x, (chatContainer.position.y - ((chatContainer.sizeDelta.y / 2)/(chatContainer.localScale.y / chatContainer.lossyScale.y))+(sizeY/2)), chatContainer.position.z), Quaternion.identity) as GameObject; //hij Instantiate de prefab op de middenpositie - de helft van de grootte van de container + de grootte van 1 dialogueBox

			box.name = (i).ToString(); //geeft box naam van huidige nummer
			boxes.Add(box); //voegt box toe aan de list boxes

			box.transform.SetParent(chatContainer); //parent box aan container
			box.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.sizeDelta.x/(chatContainer.localScale.x/chatContainer.lossyScale.x) , sizeY); //maakt box de grootte van X: de breedte van de container en van Y: sizeY

			if (lastPos != null) //als de laaste positie in is gesteld dan add dit de hoogte van de box bij de laatste positie
			{
				box.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, lastPos.anchoredPosition.y + (sizeY * (chatContainer.localScale.y/chatContainer.lossyScale.y)), 0);
			}

			lastPos = box.GetComponent<RectTransform>(); //stelt de huidige positie in om naar te referencen de volgende keer als een box word gespawned
		}

		for (int i = 0; i < boxes.Count; i++)
		{
			Add(i); //roep Add aan omdat wanneer je de functie hier in bouwt dan verscheind er een rare bug die ik niet zo makkelijk kon fixen.
		}
	}

	void Add(int cb)
	{
		boxes[cb].GetComponent<Button>().onClick.AddListener(() => Rec(int.Parse(boxes[cb].name)));
		//boxes[cb].GetComponent<Text>().text = putInBoxes[cb];
	}

	void Rec(int i)
	{
		print(i);
		chatboxCount = 0;
		lastPos = null;
		nextSearch = lastSearch+"_"+i;

		for (int I = 0; I < boxes.Count; I++) //destroyed alle boxes
		{
			Destroy(boxes[I]);
		}
		Read();
	}
}