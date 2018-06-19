using DaydreamElements.Main;
using UnityEngine;
using UnityEngine.UI;

namespace UBUSetasVR.UI
{
    public class AlphaFader : MonoBehaviour
    {
        public ScreenFade fader;

        private Graphic graphic;
        private Color c;

        // Use this for initialization
        protected virtual void Start()
        {
            graphic = transform.GetComponent<Graphic>();
            c = graphic.color;
            c.a = 0;
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            c.a = fader.Color.a;
            graphic.color = c;
        }

        private void OnDisable()
        {
            c.a = 0;
            graphic.color = c;
        }
    }
}
