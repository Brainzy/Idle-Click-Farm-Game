using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tab1Open;
     [SerializeField] private GameObject[] tab2Open;
      [SerializeField] private GameObject[] tab3Open;
      [SerializeField] private GameObject[] tab4Open;
      
      [SerializeField] private GameObject[] tab1Closed;
      [SerializeField] private GameObject[] tab2Closed;
      [SerializeField] private GameObject[] tab3Closed;
      [SerializeField] private GameObject[] tab4Closed;
      [SerializeField] private string[] textByTab;
      [SerializeField] private TextMeshProUGUI bodyText;

      [SerializeField] private Button[] closedButtonList;

      private List<GameObject[]> openObjects;
      private List<GameObject[]> closedObjects;
      
      [SerializeField] private GameObject socialCanvas;
        
      

      private void Start()
      {
          openObjects =new List<GameObject[]>(){tab1Open,tab2Open,tab3Open,tab4Open};
          closedObjects =new List<GameObject[]>(){tab1Closed,tab2Closed,tab3Closed,tab4Closed};
          for (int i = 0; i < closedButtonList.Length; i++)
          {
              var i1 = i;
              closedButtonList[i].onClick.AddListener(delegate{OpenTabButton(i1);});
          }
      }
      
      public void OpenSocialTab()
      {
          EventManager.EmitEvent("UIClick");
          if (socialCanvas.activeSelf)
          {
              socialCanvas.SetActive(false);
          }
          else
          {
              socialCanvas.SetActive(true);
          }
      }
      
      private void OpenTabButton(int i)
      {
          EventManager.EmitEvent("UIClick");
          for (int j = 0; j < openObjects.Count; j++)
          {
              if (j != i)
              {
                  for (int k = 0; k < openObjects[j].Length; k++)
                  {
                      openObjects[j][k].SetActive(false);
                  }
                  for (int k = 0; k < closedObjects[j].Length; k++)
                  {
                      closedObjects[j][k].SetActive(true);
                  }
                  
              }
              else
              {
                  bodyText.SetText(textByTab[j]);
                  for (int k = 0; k < openObjects[j].Length; k++)
                  {
                      openObjects[j][k].SetActive(true);
                  }
                  for (int k = 0; k < closedObjects[j].Length; k++)
                  {
                      closedObjects[j][k].SetActive(false);
                  }
              }
          }
      }
      
      
}
