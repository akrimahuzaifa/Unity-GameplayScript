using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TLL.Cam.StreetViewCam {
    public class InputController : MonoBehaviour
    {
        public List<TMP_InputField> inputFieldsList = new List<TMP_InputField>();
        public List<Button> buttonFieldsList = new List<Button>();
        public List<Toggle> toggleBtn = new List<Toggle>();
        public Button crossBtn;
        public GameObject inputPanelObj;

        void Reset()
        {
            int lengthInput = GameObject.FindGameObjectsWithTag("InputsControllers").Length;
            for(int i=0;i< lengthInput; i++)
            {
                inputFieldsList.Add(GameObject.FindGameObjectsWithTag("InputsControllers")[i].GetComponent<TMP_InputField>());
            }

            int lengthButton = GameObject.FindGameObjectsWithTag("ButtonsControllers").Length;
            for (int i = 0; i < lengthButton; i++)
            {
                buttonFieldsList.Add(GameObject.FindGameObjectsWithTag("ButtonsControllers")[i].GetComponent<Button>());
            }

            int lengthTogleBtn = GameObject.FindGameObjectsWithTag("ToggleBtn").Length;
            for(int i = 0; i < lengthTogleBtn; i++)
            {
                toggleBtn.Add(GameObject.FindGameObjectsWithTag("ToggleBtn")[i].GetComponent<Toggle>());
            }

            crossBtn = GameObject.FindGameObjectWithTag("CrossBtn").GetComponent<Button>();
            inputPanelObj = GameObject.FindGameObjectWithTag("InputPanelObj");
        }

        void Start()
        {
            //Input Fields Listner
            inputFieldsList[0].onValueChanged.AddListener(InputlineDeactiveWaitTime);
            inputFieldsList[1].onValueChanged.AddListener(InputlineHeight);
            inputFieldsList[2].onValueChanged.AddListener(InputlineStartYFactor);
            inputFieldsList[3].onValueChanged.AddListener(InputcamRotationSpeed);
            inputFieldsList[4].onValueChanged.AddListener(InputtransitionDuration);
            inputFieldsList[5].onValueChanged.AddListener(InputChangedFoVDuringTransition);
            inputFieldsList[6].onValueChanged.AddListener(InputDefaultFoV);
            inputFieldsList[7].onValueChanged.AddListener(InputfieldOfViewResetDuration);

            //Button Listner
            buttonFieldsList[0].onClick.AddListener(ButtonlineDeactiveWaitTime);
            buttonFieldsList[1].onClick.AddListener(ButtonlineHeight);
            buttonFieldsList[2].onClick.AddListener(ButtonlineStartYFactor);
            buttonFieldsList[3].onClick.AddListener(ButtoncamRotationSpeed);
            buttonFieldsList[4].onClick.AddListener(ButtontransitionDuration);
            buttonFieldsList[5].onClick.AddListener(ButtonChangedFoVDuringTransition);
            buttonFieldsList[6].onClick.AddListener(ButtonDefaultFoV);
            buttonFieldsList[7].onClick.AddListener(ButtonfieldOfViewResetDuration);
            crossBtn.onClick.AddListener(CrossButtonToClosePanel);

            //Toggle Listner
            toggleBtn[0].onValueChanged.AddListener(CamRotationToggleButton);
            toggleBtn[1].onValueChanged.AddListener(CamCollisionRotationToggle);
            toggleBtn[2].onValueChanged.AddListener(StreetViewCamToggleButton); //Panel On Off Toggle

            toggleBtn[2].isOn = false;
            inputPanelObj.SetActive(false);
        }

 #region InputFieldsEventsMethod
        void InputlineDeactiveWaitTime(string val)
        {
            inputFieldsList[0].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputlineHeight(string val)
        {
            inputFieldsList[1].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputlineStartYFactor(string val)
        {
            inputFieldsList[2].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputcamRotationSpeed(string val)
        {
            inputFieldsList[3].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputtransitionDuration(string val)
        {
            inputFieldsList[4].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputChangedFoVDuringTransition(string val)
        {
            inputFieldsList[5].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputDefaultFoV(string val)
        {
            inputFieldsList[6].GetComponent<TMP_InputField>().text = val.ToString();
        }

        void InputfieldOfViewResetDuration(string val)
        {
            inputFieldsList[7].GetComponent<TMP_InputField>().text = val.ToString();
        }
#endregion

 #region ButtonsFieldsEventsMethod

        void ButtonlineDeactiveWaitTime()
        {
            DragToPointTarget.dragToPointTarget_instance.lineDeactiveWaitTime = float.Parse(inputFieldsList[0].GetComponent<TMP_InputField>().text);
        }

        void ButtonlineHeight()
        {
            DragToPointTarget.dragToPointTarget_instance.lineHeight = float.Parse(inputFieldsList[1].GetComponent<TMP_InputField>().text);
        }

        void ButtonlineStartYFactor()
        {
            DragToPointTarget.dragToPointTarget_instance.lineStartYFactor = float.Parse(inputFieldsList[2].GetComponent<TMP_InputField>().text);
        }

        void ButtoncamRotationSpeed()
        {
            DragToPointTarget.dragToPointTarget_instance.camRotationSpeed = float.Parse(inputFieldsList[3].GetComponent<TMP_InputField>().text);
        }
        void ButtontransitionDuration()
        {
            DragToPointTarget.dragToPointTarget_instance.transitionDuration = float.Parse(inputFieldsList[4].GetComponent<TMP_InputField>().text);
        }
        void ButtonChangedFoVDuringTransition()
        {
            DragToPointTarget.dragToPointTarget_instance.ChangedFovDuringTransition = float.Parse(inputFieldsList[5].GetComponent<TMP_InputField>().text);
        }
        void ButtonDefaultFoV()
        {
            DragToPointTarget.dragToPointTarget_instance.DefaultFov = float.Parse(inputFieldsList[6].GetComponent<TMP_InputField>().text);
        }
        void ButtonfieldOfViewResetDuration()
        {
            DragToPointTarget.dragToPointTarget_instance.fieldOfViewResetDuration = float.Parse(inputFieldsList[7].GetComponent<TMP_InputField>().text);
        }

        void CrossButtonToClosePanel()
        {
            inputPanelObj.SetActive(false);
            toggleBtn[2].isOn = false;
        }

        void StreetViewCamToggleButton(bool onOff)
        {
            onOff = toggleBtn[2].isOn;
            inputPanelObj.SetActive(onOff);
        }

        void CamRotationToggleButton(bool onOff)
        {
            toggleBtn[0].isOn = onOff;
            DragToPointTarget.dragToPointTarget_instance.camRotateOnRightClick = toggleBtn[0].isOn;
        }

        void CamCollisionRotationToggle(bool onoff)
        {
            toggleBtn[1].isOn = onoff;
            DragToPointTarget.dragToPointTarget_instance.rotationOnCollider = toggleBtn[1].isOn;
        }

#endregion
    }
}