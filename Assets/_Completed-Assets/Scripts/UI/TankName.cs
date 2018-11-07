using UnityEngine;
using System.Collections;

public class TankName : MonoBehaviour
{
    private Camera camera;
    public string playerName;
    // Use this for initialization
    void Start()
    {
        this.camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {    //得到NPC头顶在3D世界中的坐标  
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可  
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + 1.6f, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标  
        Vector2 position = this.camera.WorldToScreenPoint(worldPosition);
        //得到真实NPC头顶的2D坐标  
        position = new Vector2(position.x, Screen.height - position.y);
        //计算NPC名称的宽高  
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(playerName));
        //设置名称显示颜色为黄色  
        GUI.color = Color.red;
        //绘制NPC名称  
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 18;
        style.fontStyle = FontStyle.BoldAndItalic;
        style.normal.textColor = Color.black;
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y, nameSize.x, nameSize.y), playerName, style);
    }
}
