using Entity.Player;
using UnityEngine;

namespace Entity.Monster {
    public class Monster : BaseEntity
    {
        [SerializeField]
        PlayerController player;
        public bool playerFound => player != null;
        void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}