using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Generator : MonoBehaviour, IInteractable
{
    public bool isOn;
    [SerializeField] Light2D connectedLight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact(){
        if(isInteractable()){
            isOn = true;
            connectedLight.intensity = 10;
        }
    }

    public bool isInteractable()
    {
        return !isOn;
    }
}
