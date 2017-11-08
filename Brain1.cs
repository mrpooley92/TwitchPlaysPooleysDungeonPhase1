using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain1 : MonoBehaviour
{
    public TwitchChatterDungeon twitchDungeonScript;
    public List<GameObject> party = new List<GameObject>();
    public List<String> viewersWaitingToJoin = new List<String>();
    public List<String> savedCharacters = new List<String>();
    public GameObject partyGO;
    public bool waitingForPlayerToAccept = false;
    public string playerWaitingFor;
    public bool confirmed = false;

    // Use this for initialization
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        ///test to see if we should try and add someone to the party
        if (party.Count < 4 && waitingForPlayerToAccept == false) {
            if (viewersWaitingToJoin.Count > 0) {
                StartCoroutine(AddSomeoneToParty(viewersWaitingToJoin[0]));
            }
        }

    }

    ///method that generates/returns a character stats string that gets used when creating the print out from $mystats
    public string GetPartyMembersString() {
        string partyMembers = "";
        if (party.Count <= 0) {
            partyMembers += "The party is empty!";
        }
        else {
            partyMembers += "The current party members are: ";
            foreach (GameObject member in party) {
                partyMembers += member.name + " ";
            }
        }
        return partyMembers;
    }

    ///method that returns true or false if the user is or isn't in the party - got tired of rewriting this test
    public bool InParty(string userName) {
        bool inParty = false;
        foreach (GameObject player in party) {
            if (player.name == userName) {
                inParty = true;
                break;
            }
        }
        return inParty;
    }

    ///method for adding a new player to the party
    public IEnumerator AddSomeoneToParty(string viewerWaitingToJoin) {
        if (party.Count < 4)
        {
            waitingForPlayerToAccept = true;
            confirmed = false;
            int numerator = -1;
            twitchDungeonScript.ConfirmJoin(viewerWaitingToJoin);
            playerWaitingFor = viewerWaitingToJoin;

            while (confirmed == false)
            {
                numerator += 1;
                if (numerator == 20)
                {
                    twitchDungeonScript.WaitingOnConfirmAlert(40, viewerWaitingToJoin);
                }
                else if (numerator == 40) {
                    twitchDungeonScript.WaitingOnConfirmAlert(20, viewerWaitingToJoin);
                }
                else if (numerator == 50)
                {
                    twitchDungeonScript.WaitingOnConfirmAlert(10, viewerWaitingToJoin);
                }

                if (numerator >= 60)
                {
                    break;
                }
       
                yield return new WaitForSeconds(1);
                Debug.Log("Tick " + numerator);
            }

            if (confirmed == true)
            {
                GameObject player;
                if (!savedCharacters.Contains(viewerWaitingToJoin))
                {
                    ///create a fresh character - pre-existing sheet not found for player
                    player = new GameObject(viewerWaitingToJoin);
                    savedCharacters.Add(viewerWaitingToJoin);
                    twitchDungeonScript.PleaseAssignClassAndGender(viewerWaitingToJoin);
                }
                else {
                    ///still need to implement saving and loading characters as gameobjects somehow
                    player = new GameObject(viewerWaitingToJoin);
                    twitchDungeonScript.PleaseAssignClassAndGender(viewerWaitingToJoin);
                }
                player.transform.parent = partyGO.transform;
                party.Add(player);
                CharacterSheet playerCharacterSheet = player.AddComponent<CharacterSheet>();
                playerCharacterSheet.ownerUsername = viewerWaitingToJoin;
            }
            else
            {
                twitchDungeonScript.NeverConfirmed(viewerWaitingToJoin);
            }
            viewersWaitingToJoin.Remove(viewerWaitingToJoin);
            waitingForPlayerToAccept = false;
            playerWaitingFor = "foobar";
            confirmed = false;
        }

        else
        {
            Debug.Log("Tried to Add Someone to the party, but the party count is " + party.Count);
        }
    }
}
