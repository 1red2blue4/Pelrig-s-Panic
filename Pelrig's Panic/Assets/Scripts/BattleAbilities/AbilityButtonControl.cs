using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonControl : MonoBehaviour
{
    GameObject selectedUnitInBattle;
    private string message = null;

    public void checkAbilityUnit()
    {
        if (!PlayerControls.abilitySelectedUnit )
        {
            return;
        }

        selectedUnitInBattle = PlayerControls.abilitySelectedUnit;


            switch (selectedUnitInBattle.name)
            {
                case "#Kent_Fantasy_Realm_temp":
                   // Debug.Log("Kent's Ability");
                    message = "Kent's ";
                    break;

                case "#Jade_Fantasy_Realm_temp":
                    //Debug.Log("Jade's Ability");
                    message = "Jade's ";
                    break;

                case "#Meda_Fantasy_Realm_temp":
                    //Debug.Log("Meda's Ability");
                    message = "Meda's ";
                    break;

                case "#Hally_Fantasy_Realm_temp":
                    //Debug.Log("Hally's Ability");
                    message = "Hally's ";
                    break;

                case "#Ed_Fantasy_Realm_temp":
                    //Debug.Log("Ed's Ability");
                    message = "Ed's ";
                    break;
                default:
                    //Debug.Log("No Unit Selected");
                    message = "No one's ";
                    break;
            }
        
    }

    public void ActivateAbilityOne()
    {
        //Debug.Log("Ability One Activated");
        checkAbilityUnit();
        Debug.Log(message + "1st Ability");
    }  


    public void ActivateAbilityTwo()
    {
        //Debug.Log("Ability Two Activated");
        checkAbilityUnit();
        Debug.Log(message + "2nd Ability");
    }

    public void ActivateAbilityThree()
    {
       // Debug.Log("Ability Three Activated");
        checkAbilityUnit();
        Debug.Log(message + "3rd Ability");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current Unit S3elected to use Abilities = " + SelectedUnitInBattle.name);
        //Debug.Log(message);
    }
}
