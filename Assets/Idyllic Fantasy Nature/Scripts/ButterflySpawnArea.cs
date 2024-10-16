using UnityEngine;

namespace IdyllicFantasyNature
{
    public class ButterflySpawnArea : MonoBehaviour
    {
        [Tooltip("the minimum cooldown for the butterfly to respawn")] [Min(0)]
        public float MinCooldown;

        [Tooltip("the maximum cooldown for the butterfly to respawn")] [Min(1)]
        public float MaxCooldown;

        public Collider Collider { get; set; }

        private void Start()
        {
            Collider = GetComponent<Collider>();
        }
    }
}