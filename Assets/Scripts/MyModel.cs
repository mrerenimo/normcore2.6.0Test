using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Normal.Realtime.Serialization;
using UnityEngine;

[RealtimeModel]
public partial class MyModel
{
    [RealtimeProperty(1, true, true)]
    private MyState _myState;

    [RealtimeProperty(2, true)]
    private RealtimeSet<SpawnPointModel> _spawnPoints;
}
