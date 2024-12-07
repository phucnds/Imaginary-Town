using ImaginaryTown.Saving;
using UnityEngine;

namespace ImaginaryTown.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SavingSystem savingSystem;
        [SerializeField] private GameObject Cameras;

        private string dataPath = "GameData";

        private void Start()
        {
            Cameras.SetActive(true);
        }

        [NaughtyAttributes.Button]
        public void Save()
        {
            savingSystem.Save(dataPath);
        }

        [NaughtyAttributes.Button]
        public void Load()
        {
            savingSystem.Load(dataPath);
        }

        [NaughtyAttributes.Button]
        private void Delete()
        {
            savingSystem.Delete(dataPath);
        }
    }
}