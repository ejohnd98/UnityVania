using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MinimapHandler : MonoBehaviour
{
    public float scale;
    public Transform topLeftTransform;
    public GameObject mapMarker;
    GameObject player;
    public PixelPerfectCamera ppc;

    // Start is called before the first frame update
    void Start(){
        player = PlayerController.instance.gameObject;
        SetLandmarks();
    }

    void SetLandmarks(){
        foreach(MinimapObject obj in FindObjectsOfType<MinimapObject>()){
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(this.transform);
            Vector3 pos = obj.transform.position - topLeftTransform.position;
            pos.z = transform.position.z;
            newObj.transform.localPosition = pos/16.0f;
            newObj.transform.localScale = new Vector3(1.0f/transform.localScale.x, 1.0f/transform.localScale.y, 1.0f);
            newObj.AddComponent<SpriteRenderer>();
            newObj.GetComponent<SpriteRenderer>().sprite = obj.minimapSprite;
            newObj.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder+1;
        }
    }

    // Update is called once per frame
    void Update(){
        Vector3 pos = player.transform.position - topLeftTransform.position;
        pos.z = transform.position.z;
        mapMarker.transform.localPosition = pos/(16.0f);
        
        for(int i = 0; i < transform.childCount; i++){
            Transform chld = transform.GetChild(i);
            chld.position = ppc.RoundToPixel(chld.position);
            chld.localScale = new Vector3(1.0f/transform.localScale.x, 1.0f/transform.localScale.y, 1.0f);
        }
        
    }

    private void LateUpdate() {
    }
}
