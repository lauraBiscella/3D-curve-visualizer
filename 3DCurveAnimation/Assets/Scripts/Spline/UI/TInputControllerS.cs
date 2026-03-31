using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace SplineCurves
{
    public class SliderInputSync : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        void Start()
        {
            inputField.text = slider.value.ToString("F2");
            slider.onValueChanged.AddListener(UpdateInputField);
            inputField.onEndEdit.AddListener(UpdateSlider);
        }

        void UpdateInputField(float value)
        {
            inputField.text = value.ToString("F2");
        }

        void UpdateSlider(string value)
        {
            if(float.TryParse(value, out float v))
            {
                v = Mathf.Clamp01(v);
                slider.value = v;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (inputField.isFocused)
            {
                inputField.ForceLabelUpdate();
                inputField.DeactivateInputField();
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}