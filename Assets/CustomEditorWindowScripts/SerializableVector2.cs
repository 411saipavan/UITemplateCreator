using UnityEngine;
public class SerializableVector2
{
    public float x,y;

    public SerializableVector2(Vector2 vector){
        this.x = vector.x;
        this.y = vector.y;
    }

    public Vector2 GetVector2(){
        return new Vector2(this.x, this.y);
    }

    public Quaternion GetQuaternion()
    {
        float angle = Mathf.Atan2(this.y, this.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
