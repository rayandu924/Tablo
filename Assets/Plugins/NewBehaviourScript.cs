using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    public string UserURL = "http://mysql-tablo.alwaysdata.net/users?"; //be sure to add a ? to your url

    //Text to display the result on
    public Text statusText;

    void Start()
    {
        StartCoroutine(GetScores());
    }

    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string name, int score)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
    }

}
