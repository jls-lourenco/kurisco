using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;

public class LoadXml : MonoBehaviour {

    public string idiome;
    public string xmlFile;

    public List<string> menu_screen;
    public List<string> options_screen;
    public List<string> credits_screen;
    public List<string> controls_screen;
    public List<string> hud_screen;
    public List<string> pause_screen;
    public List<string> gameover_screen;
    public List<string> stageclear_screen;
    public List<string> gameclear_screen;


    // Use this for initialization
    void Awake () {

        loadXmlData();
        DontDestroyOnLoad(this.gameObject);
        //SceneManager.LoadScene(1);
        
    }
	
	public void loadXmlData () {

        if (PlayerPrefs.GetString("idiome") != "")
        {
            idiome = PlayerPrefs.GetString("idiome");
        }
        
        menu_screen.Clear();
        options_screen.Clear();
        credits_screen.Clear();
        controls_screen.Clear();
        hud_screen.Clear();
        pause_screen.Clear();
        gameover_screen.Clear();
        stageclear_screen.Clear();
        gameclear_screen.Clear();

        TextAsset xmlData = (TextAsset)Resources.Load(idiome+"/"+xmlFile);

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlData.text);

        foreach (XmlNode node in xmlDocument["language"].ChildNodes)
        {
            string nodeName = node.Attributes["name"].Value;

            foreach(XmlNode n in node["textos"].ChildNodes)
            {
                switch (nodeName)
                {
                    case "menu_screen":

                        menu_screen.Add(n.InnerText);

                        break;
                    case "options_screen":

                        options_screen.Add(n.InnerText);

                        break;
                    case "credits_screen":

                        credits_screen.Add(n.InnerText);

                        break;
                    case "controls_screen":

                        controls_screen.Add(n.InnerText);

                        break;
                    case "hud_screen":

                        hud_screen.Add(n.InnerText);

                        break;

                    case "pause_screen":

                        pause_screen.Add(n.InnerText);

                        break;
                    case "gameover_screen":

                        gameover_screen.Add(n.InnerText);

                        break;
                    case "stageclear_screen":

                        stageclear_screen.Add(n.InnerText);

                        break;
                    case "gameclear_screen":

                        gameclear_screen.Add(n.InnerText);

                        break;
                }
            }
        }
	}
}
