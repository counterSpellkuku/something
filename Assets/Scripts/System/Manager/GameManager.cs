using System.Collections;
using Entity.Player;
using UnityEngine;

namespace System.Manager {
    public enum GameState {
        None,
        Starting,
        Started,
        Ended,
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}

        [SerializeField]
        bool StartGameOnAwake;

        #region Game Control Values
        public GameState state;
        public Vector2 startPos;
        public float playTime;
        
        #endregion
        [SerializeField]
        PlayerController player;
        void Awake() {
            Instance = this;
            player = FindFirstObjectByType<PlayerController>();

            if (StartGameOnAwake) {
                StartGame();
            }
        }

        public void StartGame() {
            StartCoroutine(startingGame());
        }

        IEnumerator startingGame() {
            //시작 전 초기화
            state = GameState.Starting;
            player.transform.position = startPos;
            playTime = 0;

            yield return new WaitForSeconds(1);

            state = GameState.Started;
        }

        void Update() {
            if (state == GameState.Started) {
                playTime += Time.deltaTime;

                FollowCamera();
            }
        }

        void FollowCamera() {
            CamManager.main.transform.position = new Vector3(CamManager.main.transform.position.x, Mathf.Lerp(CamManager.main.transform.position.y, player.transform.position.y, 6 * Time.deltaTime), CamManager.main.transform.position.z);
        }

        void OnDestroy() {
            Instance = null;
        }
    }
}
