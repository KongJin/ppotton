using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class AnimUtil
{
    public static async void FadeLayerWeightAsync(Animator animator, layerNames layerIndex, float targetWeight, float duration)
    {
        float startWeight = animator.GetLayerWeight((int)layerIndex);
        float elapsed = 0f;

        // duration 이 0 이하라면 즉시 세팅
        if (duration <= 0f)
        {
            animator.SetLayerWeight((int)layerIndex, targetWeight);
            return;
        }

        // 매 프레임마다 Task.Yield() 후에 돌아와서 보간
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;                                     // Unity 메인 스레드의 DeltaTime
            float t = Mathf.Clamp01(elapsed / duration);                   // 0…1
            float w = Mathf.Lerp(startWeight, targetWeight, t);            // 선형 보간
            animator.SetLayerWeight((int)layerIndex, w);                       // 바로 적용

            await Task.Yield();                                            // 다음 프레임까지 대기
        }

        // 끝점 보정
        animator.SetLayerWeight((int)layerIndex, targetWeight);
    }
}