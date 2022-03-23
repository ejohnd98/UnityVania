using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PositionFixer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = FindObjectOfType<PixelPerfectCamera>().RoundToPixel(transform.position);
    }

}
