using System.Collections.Generic;
using BuildingScripts;
using ScriptableObjectMakingScripts;
using UnityEngine;

namespace ChanglebleParameters
{
    public class JobBoardParameters : MonoBehaviour
    {
        [SerializeField] private string[] jobOrdererNames;
        [SerializeField] private int minimumAmountOfItemsPerJob = 1;
        [SerializeField] private int maximumAmountOfItemsPerJob = 3;
        [SerializeField] private int minimumAmountsOfSingleItemForJob = 1;
        [SerializeField] private int maximumAmountsOfSingleItemForJob = 5;
        [SerializeField] private int minXpForJob = 5;
        [SerializeField] private int maxXpForJob = 50;

        public BuildingAttributes[] buildingsFromWhichJobsAreMade;
        public static JobBoardParameters inst;

        private List<string> possibleJobs = new List<string>();


        private void Awake()
        {
            inst = this;
        }

        public string ReturnOrdererNames(int jobBoardStateIndex)
        {
            if (jobBoardStateIndex == 0)
            {
                return "Kindergarden";
            }

            if (jobBoardStateIndex == 1)
            {
                return "Church";
            }

            return jobOrdererNames[Random.Range(0, jobOrdererNames.Length)];
        }

        public List<string> ReturnRandomItemsRequiredForSingleJob(int jobBoardStateIndex)
        {
            MakeAListOfPossibleJobs();
            int amountToReturn = 1;
            if (jobBoardStateIndex > 1)
            {
                amountToReturn = Random.Range(minimumAmountOfItemsPerJob, maximumAmountOfItemsPerJob-1);
            }

            List<string> tempListToReturn = new List<string>();
            for (int i = 0; i < amountToReturn; i++)
            {
                tempListToReturn.Add(possibleJobs[i]);
            }

            return tempListToReturn;
        }

        public List<int> ReturnAmountsRequiredForSingleJob(int amountToReturnOnList)
        {
            List<int> tempListToReturn = new List<int>();
            for (int i = 0; i < amountToReturnOnList; i++)
            {
                tempListToReturn.Add(Random.Range(minimumAmountsOfSingleItemForJob, maximumAmountsOfSingleItemForJob-1));
            }

            return tempListToReturn;
        }

        private void MakeAListOfPossibleJobs()
        {
            for (int i = 0; i < buildingsFromWhichJobsAreMade.Length; i++)
            {
                var jobName = buildingsFromWhichJobsAreMade[i].displayName;
                possibleJobs.Add(jobName);
            }

            Utility.Shuffle(possibleJobs);
        }

        public int ReturnNumberOfJobsBasedOnState(int jobBoardStateIndex)
        {
            if (jobBoardStateIndex == -1)
            {
                return 0;
            }
            return jobBoardStateIndex <= 2 ? 1 : 6;
        }

        public int ReturnXpForSlot()
        {
            return Random.Range(minXpForJob, maxXpForJob-1);
        }

        public int CalculateCoinAwardForJob(List<string> jobItems,List<int> jobAmounts)
        {
            int amountSoFar = 0;
            for (int i = 0; i < jobItems.Count; i++)
            {
                for (int j = 0; j < buildingsFromWhichJobsAreMade.Length; j++)
                {
                    if (jobItems[i].Equals(buildingsFromWhichJobsAreMade[j].displayName))
                    {
                        amountSoFar += (jobAmounts[i] * buildingsFromWhichJobsAreMade[j].interactionResultSellPrice);
                    }
                }
            }
            return amountSoFar;
        }

        public BuildingManager.StorageLocation FindStorageLocationBasedOnString(string item)
        {
            for (int i = 0; i < buildingsFromWhichJobsAreMade.Length; i++)
            {
                var tool = buildingsFromWhichJobsAreMade[i].interactivitieLinks[0];
                for (int j = 0; j < tool.tagsItInteractsWith.Length; j++)
                {
                    if (tool.tagsItInteractsWith[j].ToString().Equals(item))
                    {
                        return tool.storageLocations[j];
                    }
                }
            }
            return BuildingManager.StorageLocation.Compost;
        }
        
    }
}