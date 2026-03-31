using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SplineCurves
{
    public class KnotControl : MonoBehaviour
    {
        [SerializeField] private CurveSystem curveSystem;
        [SerializeField] private int knotIndex;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_InputField input;

        private bool updating = false;
        private float eps = 0.01f; // piccolo margine per evitare uguaglianza

        void Start()
        {
            if(curveSystem != null && curveSystem.knots != null && curveSystem.knots.Length > knotIndex)
            {
                updating = true;
                float val = curveSystem.knots[knotIndex];
                slider.SetValueWithoutNotify(val);
                input.SetTextWithoutNotify(val.ToString("F2"));
                updating = false;
            }

            slider.onValueChanged.AddListener(OnSliderChanged);
            input.onEndEdit.AddListener(OnInputChanged);
        }

        private void OnSliderChanged(float value)
        {
            if (updating) return;
            updating = true;

            // limiti dinamici
            float lower = (knotIndex == 3) ? float.MinValue : curveSystem.knots[knotIndex - 1] + eps;
            float upper = (knotIndex == 7) ? float.MaxValue : curveSystem.knots[knotIndex + 1] - eps;

            value = Mathf.Clamp(value, lower, upper);

            curveSystem.OnKnotChanged(knotIndex, value);

            // aggiorna UI senza triggerare ricorsione
            slider.SetValueWithoutNotify(curveSystem.knots[knotIndex]);
            input.SetTextWithoutNotify(curveSystem.knots[knotIndex].ToString("F2"));

            updating = false;
        }

        private void OnInputChanged(string text)
        {
            if (updating) return;

            if (!float.TryParse(text, out float value))
            {
                // reset al valore precedente se input non valido
                updating = true;
                input.SetTextWithoutNotify(curveSystem.knots[knotIndex].ToString("F2"));
                updating = false;
                return;
            }

            updating = true;

            float lower = (knotIndex == 3) ? float.MinValue : curveSystem.knots[knotIndex - 1] + eps;
            float upper = (knotIndex == 7) ? float.MaxValue : curveSystem.knots[knotIndex + 1] - eps;

            value = Mathf.Clamp(value, lower, upper);

            curveSystem.OnKnotChanged(knotIndex, value);

            slider.SetValueWithoutNotify(curveSystem.knots[knotIndex]);
            input.SetTextWithoutNotify(curveSystem.knots[knotIndex].ToString("F2"));

            updating = false;
        }
    }
}