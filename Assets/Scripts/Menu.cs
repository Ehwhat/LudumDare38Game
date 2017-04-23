using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    
    public void LoadScene(int level)
        // can I just write onclick? Hum. Purhaps I should not attempt this... yet. I remember far too little. 
    {
        Application.LoadLevel(level);
    }
		
}
