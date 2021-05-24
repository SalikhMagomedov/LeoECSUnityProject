using Systems;
using Components;
using Leopotam.Ecs;
using UnityEngine;

internal sealed class Startup : MonoBehaviour {
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject spawnPrefab;

    private EcsWorld _world;
    private EcsSystems _systems;

    private void Start () {
        // void can be switched to IEnumerator for support coroutines.
            
        _world = new EcsWorld ();
        
        for (var i = 0; i < spawnCount; i++)
        {
            var entity = _world.NewEntity();
            var moveRightComponent = new MoveRightComponent {Speed = .1f};
            
            var randomPoint = Random.insideUnitCircle * spawnRadius;
            var t = transform;
            var spawnedInstance = Instantiate(spawnPrefab,
                t.position + new Vector3(randomPoint.x, randomPoint.y), Quaternion.identity, t);
            spawnedInstance.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            moveRightComponent.Transform = spawnedInstance.transform;
            entity.Replace(moveRightComponent);
        }
        
        _systems = new EcsSystems (_world);
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
        _systems
            // register your systems here, for example:
            // .Add (new TestSystem1 ())
            // .Add (new TestSystem2 ())
            .Add(new MoveRightSystem())
                
            // register one-frame components (order is important), for example:
            // .OneFrame<TestComponent1> ()
            // .OneFrame<TestComponent2> ()
                
            // inject service instances here (order doesn't important), for example:
            // .Inject (new CameraService ())
            // .Inject (new NavMeshSupport ())
            .Init ();
    }

    private void Update () {
        _systems?.Run ();
    }

    private void OnDestroy ()
    {
        if (_systems == null) return;
        
        _systems.Destroy ();
        _systems = null;
        _world.Destroy ();
        _world = null;
    }
}