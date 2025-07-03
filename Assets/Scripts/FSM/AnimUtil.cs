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

        // duration �� 0 ���϶�� ��� ����
        if (duration <= 0f)
        {
            animator.SetLayerWeight((int)layerIndex, targetWeight);
            return;
        }

        // �� �����Ӹ��� Task.Yield() �Ŀ� ���ƿͼ� ����
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;                                     // Unity ���� �������� DeltaTime
            float t = Mathf.Clamp01(elapsed / duration);                   // 0��1
            float w = Mathf.Lerp(startWeight, targetWeight, t);            // ���� ����
            animator.SetLayerWeight((int)layerIndex, w);                       // �ٷ� ����

            await Task.Yield();                                            // ���� �����ӱ��� ���
        }

        // ���� ����
        animator.SetLayerWeight((int)layerIndex, targetWeight);
    }
}