using System;
using ImaginaryTown.Saving;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class Character : MonoBehaviour, ISaveable
{
    [Serializable]
    struct MoverSaveData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState()
    {
        MoverSaveData data = new MoverSaveData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {
        MoverSaveData data = (MoverSaveData)state;
        transform.position = data.position.ToVector();
        transform.eulerAngles = data.rotation.ToVector();
    }
}
