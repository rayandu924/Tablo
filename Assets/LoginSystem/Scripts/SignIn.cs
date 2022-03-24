using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Net.Mail;
using TMPro;

public class SignIn : MonoBehaviour
{
    // Start is called before the first frame update
    public SmtpClient client;
    public MailAddress from;

    public TMP_InputField InputEmail;
    public TMP_InputField InputVerificationCode;

    void Start()
    {
        // Command-line argument must be the SMTP host.
        client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential(
            "no.reply.tablo@gmail.com", 
            "@Tablo112");
        client.EnableSsl = true;
        // Specify the email sender.
        // Create a mailing address that includes a UTF8 character
        // in the display name.
        from = new MailAddress(
            "no.reply.tablo@gmail.com",
            "Tablo",
            System.Text.Encoding.UTF8);
    }

    public void SendEmail()
    {
        int VerificationCode = Random.Range(100000, 999999);
        // Set destinations for the email message.
        MailAddress to = new MailAddress(InputEmail.text);
        // Specify the message content.
        MailMessage message = new MailMessage(from, to);
        message.Body = "Hello, Your verification code is "+ VerificationCode;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.Subject = "test message 1";
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        // Set the method that is called back when the send operation ends.
        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        // The userState can be any object that allows your callback
        // method to identify this send operation.
        // For this example, the userToken is a string constant.
        string userState = "test message1";
        client.SendAsync(message, userState);
    }

    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        // Get the unique identifier for this asynchronous operation.
        string token = (string)e.UserState;

        if (e.Cancelled)
        {
            Debug.Log("Send canceled "+ token);
        }
        if (e.Error != null)
        {
            Debug.Log("[ "+token+" ] " + " " + e.Error.ToString());
        }
        else
        {
            Debug.Log("Message sent.");
        }
    }
}

