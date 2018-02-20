using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPressHandler : MonoBehaviour {

    public UnityEvent ACTION;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tip" && ACTION != null)
            ACTION.Invoke();
    }
}
