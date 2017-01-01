using UnityEngine;
using System.Collections;

public class CUIBillBoard : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () {
        Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);
        transform.rotation = rot;
	}
}
