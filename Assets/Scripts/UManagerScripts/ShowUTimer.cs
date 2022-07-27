using System;
using System.Text;
using BuildingScripts;
using GameManagerScripts;
using TigerForge;
using TMPro;
using UnityEngine;

namespace UManagerScripts
{
    public class ShowUTimer : MonoBehaviour
    {
        [SerializeField] private GameObject timerParent;
        [SerializeField] private RectTransform barFillScale;
        [SerializeField] private TextMeshProUGUI interactionResultName;
        [SerializeField] private TextMeshProUGUI timeRemaining;

        public static ShowUTimer inst;
        private bool timerInFocus;
        private float oneSecondUpdate;
        private float maxTime;
        private Vector3 currentScale;
        private Building building;
        private StringBuilder stringBuilder = new StringBuilder();

        private const string second = " sec ";
        private const string minute = " min ";
        private const string hour = " h ";


        private void Awake()
        {
            inst = this;
        }

        private void Update()
        {
            if (timerInFocus == false) return;
            oneSecondUpdate -= Time.deltaTime;
            if (oneSecondUpdate < 0)
            {
                oneSecondUpdate = 1;
                UpdateTimeAndScale();
            }
        }

        private void UpdateTimeAndScale()
        {
            var currentTime = building.growTimer;
            barFillScale.localScale = new Vector3(currentTime / maxTime, currentScale.y, currentScale.z);
            timeRemaining.SetText(FormatTime(currentTime));
            if (currentTime < 0.1f)
            {
                timerInFocus = false;
                timerParent.SetActive(false);
            }
        }

        private string FormatTime(float time)
        {
            stringBuilder.Clear();
            if (time <= 60)
            {
                time = Mathf.RoundToInt(time);
                return stringBuilder.Append(time).Append(second).ToString();
            }
            if (time > 60 && time<3600)
            {
                return stringBuilder.Append(TimeSpan.FromSeconds(time).Minutes).Append(minute).Append(TimeSpan.FromSeconds(time).Seconds).Append(second).ToString();
            }
            else
            {
                return stringBuilder.Append(TimeSpan.FromSeconds(time).Hours).Append(hour).Append(TimeSpan.FromSeconds(time).Minutes).Append(minute).ToString();
            }
        }

        public void ShowGrowTimer(Building targetBuilding)
        {
            EventManager.EmitEvent("UIClick");
            building = targetBuilding;
            timerInFocus = true;
            timerParent.transform.position = GameManager.inst.cam.WorldToScreenPoint(building.transform.position);
            interactionResultName.SetText(building.buildingAttributes.interactionResultName);
            maxTime = building.buildingAttributes.myGrowableTimers[building.myPrefabIndex];
            currentScale = barFillScale.localScale;
            oneSecondUpdate = 1;
            timerParent.SetActive(true);
        }
    }
}
