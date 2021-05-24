using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems {
    internal sealed class MoveRightSystem : IEcsRunSystem {
        // auto-injected fields.
        private readonly EcsFilter<MoveRightComponent> _filter = null; 
        
        void IEcsRunSystem.Run ()
        {
            var deltaTime = Time.deltaTime;
            foreach (var i in _filter)
            {
                ref var moveRight = ref _filter.Get1(i);
                moveRight.Transform.Translate(Vector3.right * (moveRight.Speed * deltaTime));
            }
        }
    }
}