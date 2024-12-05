using UnityEngine;

namespace ImaginaryTown.Ultis
{
    [CreateAssetMenu(fileName = "Readme", menuName = "ImaginaryTown/Utils/Readme", order = 0)]
    public class Readme : ScriptableObject
    {
        [NaughtyAttributes.Button]
        public void OrganizeHierarchy()
        {
            new GameObject("--- ENVIRONEMENT ---");
            new GameObject(" ");

            new GameObject("--- GAMEPLAY ---");
            new GameObject(" ");

            new GameObject("--- UI ---");
            new GameObject(" ");

            new GameObject("--- MANAGERS ---");
        }

        [NaughtyAttributes.Button]
        public void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}