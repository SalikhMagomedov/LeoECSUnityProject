using Components;
using Leopotam.Ecs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Systems
{
    internal sealed class MoveRightSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        // auto-injected fields.
        private readonly EcsFilter<MoveRightComponent> _filter = null;
        private NativeArray<float> _speeds;
        private TransformAccessArray _transforms;
        private MoveJob _job;

        public void Init()
        {
            _speeds = new NativeArray<float>(_filter.GetEntitiesCount(), Allocator.Persistent);
            _transforms = new TransformAccessArray(_filter.GetEntitiesCount());
            _job = new MoveJob {Speeds = _speeds};

            foreach (var i in _filter)
            {
                ref var moveComponent = ref _filter.Get1(i);
                _speeds[i] = moveComponent.Speed;
                _transforms.Add(moveComponent.Transform);
            }
        }

        void IEcsRunSystem.Run()
        {
            var deltaTime = Time.deltaTime;
            
            _job.DeltaTime = deltaTime;
            
            var handle = _job.Schedule(_transforms);

            handle.Complete();
        }

        public void Destroy()
        {
            _speeds.Dispose();
            _transforms.Dispose();
        }
    }

    public struct MoveJob : IJobParallelForTransform
    {
        [ReadOnly] public float DeltaTime;
        [ReadOnly] public NativeArray<float> Speeds;

        public void Execute(int index, TransformAccess transform)
        {
            var tempPosition = transform.position;
            tempPosition += Vector3.right * (Speeds[index] * DeltaTime);
            transform.position = tempPosition;
        }
    }
}