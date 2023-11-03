using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TransformSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Transform transform = (Transform)obj;
        info.AddValue("position", transform.position);
        info.AddValue("rotation", transform.rotation);
        info.AddValue("scale", transform.localScale);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Transform transform = (Transform)obj;
        transform.position = (Vector3)info.GetValue("position", typeof(Vector3));
        transform.rotation = (Quaternion)info.GetValue("rotation", typeof(Quaternion));
        transform.localScale = (Vector3)info.GetValue("scale", typeof(Vector3));
        obj = transform;
        return obj;
    }
}
