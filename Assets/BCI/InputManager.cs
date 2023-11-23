using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData {
    public InputValidity validity;
    public InputType type;
    public float confidence;
    public int inputNumber;
}

public enum InputValidity {
    Accepted,
    Rejected
}

public enum InputType {
    KeySequence,
    MotorImagery,
    BlinkDetection,
    FabInput
}


public class InputManager : MonoBehaviour

{
    private LanguageVersionManager LVManager;
    // Start is called before the first frame update
    void Start()
    {
        LVManager = GameObject.Find("Language/VersionManager").GetComponent<LanguageVersionManager>();

        if(LVManager != null)
        {
            OpenBCIInput online = GetComponent<OpenBCIInput>();
            SimBCIInput sim = GetComponent<SimBCIInput>();
            if(LVManager.BCIPref == true) // Online
            {
                online.enabled = true;
                sim.enabled = false;
            }
            else // Simulated
            {
                online.enabled = false;
                sim.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
