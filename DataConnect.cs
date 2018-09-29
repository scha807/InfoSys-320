using UnityEngine;
//using Pathfinding.Serialization.JsonFx; //old method
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;

public class DataConnect : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject myQuestion;

    string SpacemanURL = "https://scha807.azurewebsites.net/tables/Spaceman?zumo-api-version=2.0.0";
    string QuestionURL = "https://scha807.azurewebsites.net/tables/Question?zumo-api-version=2.0.0";
    public string answer;
    string jsonResponse;

    void Start()
    {
        WWW spacemanWww = new WWW(SpacemanURL);
        WWW questionWww = new WWW(QuestionURL);
        while (spacemanWww.isDone == false || questionWww.isDone == false) ;
        string spacemanjsonResponse = spacemanWww.text;
        string questionjsonResponse = questionWww.text;

        if ((string.IsNullOrEmpty(spacemanjsonResponse)) || (string.IsNullOrEmpty(questionjsonResponse)))
        {
            return;
        }
 
        Spaceman[] Spacemen = JsonConvert.DeserializeObject<Spaceman[]>(spacemanjsonResponse);
        Question[] Questions = JsonConvert.DeserializeObject<Question[]>(questionjsonResponse);

        int i = 0;
        foreach (Spaceman Spaceman in Spacemen)
        {
            float x = Spaceman.X;
            float y = Spaceman.Y;
            float z = Spaceman.Z;

            var newObject = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
            newObject.transform.Rotate(0, 180, 0);
            newObject.transform.Find("New Text").GetComponent<TextMesh>().text = Spaceman.SpacemanName;
            i++;
        }

        foreach (Question Question in Questions)
        {
            float x = Question.X;
            float y = Question.Y;
            float z = Question.Z;
            answer = Question.Answer;

            var newObject2 = (GameObject)Instantiate(myQuestion, new Vector3(x, y, z), Quaternion.identity);
            newObject2.transform.Find("Question Text").GetComponent<TextMesh>().text = Question.QuestionDescription;
        }
    }

    IEnumerator GetData()
    {
        Debug.Log("Getting  Data");
        UnityWebRequest www = UnityWebRequest.Get(SpacemanURL);       
        www.SendWebRequest();
        //yield return www.SendWebRequest();
        {
            Debug.Log("Retrieved  Data For " + SpacemanURL);
            new WaitForSeconds(40);
            
            jsonResponse = www.downloadHandler.text;
            yield return new WaitForSeconds(1);
            //yield return new WaitForSeconds(20);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
