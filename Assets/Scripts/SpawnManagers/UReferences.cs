using System.Collections.Generic;
using ButtonScripts;
using PathCreation;
using UnityEngine;

namespace SpawnManagers
{
    public class UReferences : MonoBehaviour
    {
        [SerializeField] private UImageBuilding[] uScripts;
        public PathCreator incommingLandPath;
        public PathCreator outgoingLandPath;

        public static UReferences inst;
        private Dictionary<string, UImageBuilding> dict= new Dictionary<string, UImageBuilding>();

        private void Awake()
        {
            inst = this;
            for (int i = 0; i < uScripts.Length; i++) // make dict for string search
            {
                dict.Add(uScripts[i].name,uScripts[i]);
            }
        }
        public UImageBuilding ReturnUScript(string nameOfUObject)
        {
            return dict[nameOfUObject];
        }
    
    
    }
}
