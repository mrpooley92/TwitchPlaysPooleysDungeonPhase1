using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using TwitchChatter;

public class TwitchChatterDungeon : MonoBehaviour {

    public Brain1 brainScript;

	// Use this for initialization
	void Start () {


        ///viewer chat commands so far = $join party, $confirm, $list party, $leave party, $assign gender class (e.g. '$assign female wizard'), $mystats
        ///joke commands = $kill mrpooley92, $attack mrpooley92

        

        // Add a chat listener.
        TwitchChatClient.singleton.AddChatListener(OnChatMessage);

        // Set your credentials. If you're not planning on sending messages,
        // you can remove these lines.
        TwitchChatClient.singleton.userName = "PooleyDungeonBot";
        TwitchChatClient.singleton.oAuthPassword = "whatsthepassword";

        // Join some channels.
        TwitchChatClient.singleton.JoinChannel("mrpooley92");

        // If you set your credentials and you'd like to receive whispers,
        //  call EnableWhispers to allow for sending/receiving whispers.
        TwitchChatClient.singleton.EnableWhispers();

        // Then, add any whisper listeners you'd like.
        TwitchChatClient.singleton.AddWhisperListener(OnWhisper);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // You'd define your chat message callback like this:
    public void OnChatMessage(ref TwitchChatMessage msg)
    {
        ///test to make sure this is a PooleyDungeon command (starts with '$')
        if (msg.chatMessagePlainText[0].ToString() == "$")
        {
            ///implementation of $join party command
            if (msg.chatMessagePlainText.Equals("$join party"))
            {
                Debug.Log("FIRED");
                if (!brainScript.viewersWaitingToJoin.Contains(msg.userName))
                {

                    brainScript.viewersWaitingToJoin.Add(msg.userName);
                    TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " has joined the waiting list!");

                }
            }
            //////implementation of $confirm command to join the party
            else if (msg.chatMessagePlainText.Equals("$confirm") && brainScript.playerWaitingFor == msg.userName && brainScript.waitingForPlayerToAccept == true)
            {
                TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " joined the party!");
                brainScript.confirmed = true;
            }
            ///implementation of $list party command
            else if (msg.chatMessagePlainText.Equals("$list party"))
            {
                ListPartyMembers();
            }

            ///implementation of $leave party command
            else if (msg.chatMessagePlainText.Equals("$leave party"))
            {
                bool inParty = false;
                foreach (GameObject player in brainScript.party)
                {
                    if (player.name == msg.userName)
                    {
                        brainScript.party.Remove(player);
                        GameObject.Destroy(player);
                        inParty = true;
                        TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " left the party.");
                        break;
                    }
                }
                if (inParty == false)
                {
                    TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " you're not in the party!");
                }
            }

            //////implementation of $mystats command
            else if (msg.chatMessagePlainText.Equals("$mystats")) {
                if (brainScript.InParty(msg.userName)) {
                    string statString = "";
                    foreach (GameObject player in brainScript.party) {
                        if (player.name == msg.userName) {
                            statString = player.GetComponent<CharacterSheet>().CharacterStats();
                            break;
                        }
                    }
                    TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " your stats are: " + statString);
                }
                else {
                    TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " you're not in the party!");
                }
            }
            
            ///implementation of $assign gender class command
            else if (msg.chatMessagePlainText.Contains("$assign"))
            {
                bool inParty = false;
                foreach (GameObject player in brainScript.party)
                {
                    if (player.name == msg.userName)
                    {
                        inParty = true;
                        CharacterSheet playerSheet = player.GetComponent<CharacterSheet>();

                        if (playerSheet.classIsAssigned == true)
                        {
                            TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " you have already assigned your class and gender!");
                        }
                        else
                        {
                            string[] msgSplit = msg.chatMessagePlainText.Split(' ');
                            if (msgSplit.Length != 3 || (msgSplit[2] != "warrior" && msgSplit[2] != "rogue" && msgSplit[2] != "wizard") || (msgSplit[1] != "male" && msgSplit[1] != "female"))
                            {
                                TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " I don't think you typed that right.");
                            }
                            else
                            {
                                playerSheet.characterClass = msgSplit[2];
                                playerSheet.gender = msgSplit[1];
                                playerSheet.classIsAssigned = true;
                                playerSheet.AssignValues();
                                TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " assignment successful. You are a " + playerSheet.gender + " " + playerSheet.characterClass);
                            }
                        }

                        break;
                    }
                }
                if (inParty == false)
                {
                    TwitchChatClient.singleton.SendMessage("mrpooley92", msg.userName + " you're not in the party!");
                }
            }




            ///implementation of joke command
            else if (msg.chatMessagePlainText.Equals("$kill mrpooley92"))
            {
                TwitchChatClient.singleton.SendMessage("mrpooley92", "Yea, nice try " + msg.userName);
            }

            ///implementation of joke command
            else if (msg.chatMessagePlainText.Equals("$attack mrpooley92"))
            {
                TwitchChatClient.singleton.SendMessage("mrpooley92", "Ouch " + msg.userName + " . Very ouch.");
            }
        }



        ///if message did not start with '$'
        else {
            //DONT RESPOND BECAUSE WE'RE ONLY ACCEPTING COMMANDS THAT START WITH $
        }
    }

    public void ConfirmJoin(string userName) {
        TwitchChatClient.singleton.SendMessage("mrpooley92", userName + " is up next! " + userName + " please confirm by typing '$confirm'. You have 60 seconds to do so.");
    }

    public void NeverConfirmed(string userName) {
        TwitchChatClient.singleton.SendMessage("mrpooley92", "Did not receive confirmation from " + userName + " . Please rejoin the waiting list.");
    }

    public void ListPartyMembers() {
        TwitchChatClient.singleton.SendMessage("mrpooley92", brainScript.GetPartyMembersString());
    }

    public void WaitingOnConfirmAlert(int secondsLeft, string viewerWaitingOn) {
        TwitchChatClient.singleton.SendMessage("mrpooley92", viewerWaitingOn + " you have " + secondsLeft + " seconds left to confirm. Confirm by typing '$confirm'.");
    }

    public void PleaseAssignClassAndGender(string userName) {
        TwitchChatClient.singleton.SendMessage("mrpooley92", userName + " please assign your gender and class by typing '$assign gender class'. (E.g. '$assign male wizard')");
    }


    // You'd define your whisper callback like this:
    public void OnWhisper(ref TwitchChatMessage msg)
    {
        
        // Do something with the whisper here.
    }

    void Cleanup()
    {
        // When you're done, leave the channels and remove the chat listeners.
        TwitchChatClient.singleton.LeaveChannel("mrpooley92");
        TwitchChatClient.singleton.RemoveChatListener(OnChatMessage);

        // Also remove any whisper listeners you've added.
        TwitchChatClient.singleton.RemoveWhisperListener(OnWhisper);
    }
}
