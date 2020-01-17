using UnityEngine;

public class InterfaceManager : MonoBehaviour {
    public PlayerController p1;
    public PlayerController p2;

    void Update() {
        p1.Move(Input.GetAxis("Horizontal"));
        p2.Move(Input.GetAxis("Vertical"));
        
        if(Input.GetKeyDown(KeyCode.Space))
            p1.Jump();
        
        if(Input.GetKeyDown(KeyCode.Return))
            p2.Jump();
    }
}