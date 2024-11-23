using UnityEngine;
using UnityEngine.UI;

namespace System.Manager {
    public class UIManager : MonoBehaviour {
        public Image skill1Col, skill2Col, skill3Col, skill4Col;
        public Image weaponImg, weaponBack;
        public UnityEngine.UI.Text waveInfo, waveInfoBack;
        public UnityEngine.UI.Text hpText, hpTextBack;
        public Slider hpRate;

        public static UIManager Instance {get; private set;}

        void Awake() {
            Instance = this;
        }
    }
}