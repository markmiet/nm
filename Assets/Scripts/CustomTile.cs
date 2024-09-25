using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomTile", menuName = "Tiles/CustomTile")]
public class CustomTile : TileBase
{
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        
        tileData.sprite = sprite;
        float scale=Random.Range(0.5f, 1.0f);


        //  public Vector3 scale = Vector3.one; // Scale of the tile
        Vector3 scalev = new Vector3(scale, scale, 1.0f);

       Vector3 offset= new Vector3(scale/1.0f, scale/1.0f, 1.0f);

        

       // tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scalev);
       // tileData.transform = Matrix4x4.TRS(offset, Quaternion.identity, new Vector3(1,1,1));
      //  tileData.transform = Matrix4x4.TRS(offset, Quaternion.identity, scalev);

        // Optionally, set additional properties here (e.g., color, transform)
       
    }

}
