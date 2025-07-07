using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MagneticFieldMovingVolume : NetworkBehaviour
{
    float targetTime = 0;
    float speed=10;

    Vector3 targetVolume;
    Vector3 startVolume;

    Vector3 targetPosition;
    Vector3 startPosition;

    float curAchievement;

    public override void Spawned()
    {
        base.Spawned();

        startVolume = targetVolume = transform.localScale;
    }
    //   

    
    public override void FixedUpdateNetwork()
    {
        if (targetTime < Runner.SimulationTime)
        {
            targetTime = Runner.SimulationTime + 5;// 10초에 한번씩 갱신
            startPosition = targetPosition;
            startVolume = targetVolume;
            curAchievement = 0;

            targetVolume *= 0.7f;
            var harf = targetVolume.x / 2f;

            var randomDirection = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f)).normalized;
            targetPosition = randomDirection*harf + targetPosition;


        }
        if(curAchievement<1f)
        {
            curAchievement += Runner.DeltaTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition, curAchievement);
            transform.localScale = Vector3.Lerp(startVolume, targetVolume, curAchievement);
        }




    }

}
