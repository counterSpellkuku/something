using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace System.Manager {
    public class CamManager : MonoBehaviour
    {
        public static CamManager main;
        public CinemachineCamera cam;
        public CinemachineBasicMultiChannelPerlin noise;
        public CinemachineCameraOffset camOffset;
        float orSize_d;
        float dutch_d;

        IEnumerator dutchRoutine = null;
        IEnumerator offRoutine = null;

        public bool autoSpector;

        private void Awake() {
            cam = GetComponent<CinemachineCamera>();
            camOffset = GetComponent<CinemachineCameraOffset>();
            noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

            main = this;

            orSize_d = cam.Lens.OrthographicSize;
            dutch_d = cam.Lens.Dutch;
        }

        private void Update() {
        }

        void ClearRoutine(ref IEnumerator routine) {
            if (routine != null) {
                StopCoroutine(routine);

                routine = null;
            }
        }

        public void CloseUp(float orSize, float dutch, float dur = 0) {
            ClearRoutine(ref dutchRoutine);
            dutchRoutine = _closeUp(orSize, dutch, dur);

            StartCoroutine(dutchRoutine);
        }
        public void CloseOut(float dur = 0) {
            ClearRoutine(ref dutchRoutine);
            dutchRoutine = _closeOut(dur);

            StartCoroutine(dutchRoutine);
        }
        public void Offset(Vector2 off, float dur = 0) {
            ClearRoutine(ref offRoutine);

            offRoutine = _offset(off, dur);

            StartCoroutine(offRoutine);
        }

        public void Shake(float strength = 1, float dur = 0.05f)
        {
            StartCoroutine(_shake(strength, dur));
        }

        IEnumerator _closeUp(float orSize, float dutch, float dur) {
            if (dur > 0) {
                float dSize = cam.Lens.OrthographicSize, dDutch = cam.Lens.Dutch;

                for (int i = 1; i <= 20; i++) {
                    cam.Lens.OrthographicSize = dSize - (dSize - orSize) / 20 * i;
                    cam.Lens.Dutch = dDutch - (dDutch - dutch) / 20 * i;

                    yield return new WaitForSeconds(dur / 20);
                }
            }

            cam.Lens.OrthographicSize = orSize;
            cam.Lens.Dutch = dutch;

            dutchRoutine = null;
        }

        IEnumerator _closeOut(float dur) {
            if (dur > 0) {
                float dSize = cam.Lens.OrthographicSize, dDutch = cam.Lens.Dutch;

                for (int i = 1; i <= 20; i++) {
                    cam.Lens.OrthographicSize = dSize + (orSize_d - dSize) / 20 * i;
                    cam.Lens.Dutch = dDutch + (dutch_d - dDutch) / 20 * i;

                    yield return new WaitForSeconds(dur / 20);
                }
            }
            
            cam.Lens.OrthographicSize = orSize_d;
            cam.Lens.Dutch = dutch_d;

            dutchRoutine = null;
        }

        IEnumerator _offset(Vector3 off, float dur = 0) {
            if (dur > 0) {
                Vector2 beforeOff = camOffset.Offset;

                for (int i = 1; i <= 30; i++) {
                    camOffset.Offset = new Vector3(
                        beforeOff.x - (beforeOff.x - off.x) / 30 * i,
                        beforeOff.y - (beforeOff.y - off.y) / 30 * i
                    );

                    yield return new WaitForSeconds(dur / 30);
                }
            }

            camOffset.Offset = off;

            offRoutine = null;
        }

        IEnumerator _shake(float strength, float dur)
        {
            noise.AmplitudeGain = strength;
            noise.FrequencyGain = strength;

            yield return new WaitForSeconds(dur);

            noise.AmplitudeGain = 0;
            noise.FrequencyGain = 0;
        }
    }
}