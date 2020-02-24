using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMovement : MonoBehaviour
{
    private Renderer WallRenderer => GetComponent<Renderer>();
    private Material WallMaterial => WallRenderer.material;
    private const float Speed = 3f;
    [SerializeField] private bool invert;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invert)
        {
            Vector2 offset = new Vector2(Time.deltaTime * Speed, 0);
            WallMaterial.mainTextureOffset -= offset;
        }
        else
        {
            Vector2 offset = new Vector2(Time.deltaTime * Speed, 0);
            WallMaterial.mainTextureOffset += offset;
        }
    }
}
