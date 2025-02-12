using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Camera cameraPlayer;
    public Material[] sprites;
    public Transform player;

    private int length;

    // Start is called before the first frame update
    void Start()
    {
        length = sprites.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Sprite mira a camara
        transform.LookAt(new Vector3(cameraPlayer.transform.position.x, transform.position.y, cameraPlayer.transform.position.z), Vector2.up);
        transform.rotation *=  Quaternion.Euler(90, 0, 0);

        ChangeSprite();
    }

    public void ChangeSprite()
    {
        float changeValue = 360 / (length);
        float rawAngle = Mathf.DeltaAngle(player.eulerAngles.y, cameraPlayer.transform.eulerAngles.y);
        float clockwiseAngle = (rawAngle + 360) % 360;
        int index = Mathf.FloorToInt(clockwiseAngle / changeValue); 
        index = Mathf.Clamp(index, 0, length - 1);

        GetComponent<Renderer>().material = sprites[index];
    }
}
