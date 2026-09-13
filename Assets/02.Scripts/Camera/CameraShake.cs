using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 실제로 흔들릴 대상 Transform
    // 보통 Main Camera의 Transform을 연결
    [SerializeField] private Transform _shakeTarget;

    // 시간에 따른 흔들림 강도를 조절하는 곡선
    // 기본값은 처음에 강하고 시간이 지날수록 약해지도록 설정
    [SerializeField]
    private AnimationCurve _shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    // 현재 실행 중인 Camera Shake 코루틴을 저장
    // 연속으로 Shake가 호출될 경우 기존 코루틴을 중지하기 위해 사용
    private Coroutine _shakeCoroutine;

    // Perlin Noise에서 사용할 X축, Y축 시작 위치
    // 서로 다른 값을 사용해 X축과 Y축 흔들림이 동일해지는 것을 방지
    private float _seedX;
    private float _seedY;

    private void Awake()
    {
        // 실행할 때마다 다른 흔들림 패턴이 나오도록
        // Perlin Noise의 시작 위치를 랜덤하게 설정
        _seedX = Random.Range(0f, 100f);
        _seedY = Random.Range(0f, 100f);
    }

    // 외부에서 Camera Shake를 실행할 때 호출하는 메서드
    // duration  : 흔들림이 지속되는 시간
    // strength  : 흔들림의 크기
    // frequency : 흔들림의 빠르기
    public void Play(float duration, float strength, float frequency)
    {
        // 이미 Camera Shake가 실행 중이라면
        if (_shakeCoroutine != null)
        {
            // 기존 Shake 코루틴을 중지
            StopCoroutine(_shakeCoroutine);
        }

        // 새로운 Camera Shake 코루틴 시작
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, strength, frequency));
    }

    // 실제 카메라 흔들림을 처리하는 코루틴
    private IEnumerator ShakeCoroutine(float duration, float strength, float frequency)
    {
        // 흔들림이 끝난 뒤 원래 위치로 복구하기 위해
        // 시작 시점의 localPosition을 저장
        Vector3 originalPosition = _shakeTarget.localPosition;

        // 현재까지 진행된 Shake 시간
        float elapsedTime = 0f;

        // 지정한 duration 동안 반복
        while (elapsedTime < duration)
        {
            // 현재 Shake 진행도를 0 ~ 1 범위로 변환
            // 0 = 시작, 1 = 종료
            float normalizedTime = elapsedTime / duration;

            // AnimationCurve를 이용해
            // 현재 시점의 흔들림 감쇠값 계산
            // 시간이 지날수록 흔들림이 약해짐
            float damping = _shakeCurve.Evaluate(normalizedTime);

            // Perlin Noise를 얼마나 빠르게 이동할지 결정
            // unscaledTime을 사용해 Hit Stop 중에도 흔들림이 진행되도록 함
            float noiseTime = Time.unscaledTime * frequency;

            // PerlinNoise의 반환값은 0 ~ 1이므로
            // -1 ~ 1 범위로 변환하여 좌우 흔들림 값 생성
            float x = Mathf.PerlinNoise(_seedX, noiseTime) * 2f - 1f;

            // Y축도 별도의 Seed를 사용하여
            // 위아래 흔들림 값 생성
            float y = Mathf.PerlinNoise(_seedY, noiseTime) * 2f - 1f;

            // 흔들림 방향에 Strength와 감쇠값을 적용하여
            // 최종 Camera Shake Offset 계산
            Vector3 offset = strength * damping * new Vector3(x, y, 0f);

            // 원래 위치에 흔들림 Offset을 더해 카메라 이동
            _shakeTarget.localPosition = originalPosition + offset;

            // Hit Stop에서 Time.timeScale이 0이 되어도
            // Camera Shake 시간이 계속 흐르도록 unscaledDeltaTime 사용
            elapsedTime += Time.unscaledDeltaTime;

            // 다음 프레임까지 코루틴 실행을 잠시 중단
            yield return null;
        }

        // Camera Shake가 끝나면 원래 위치로 복구
        _shakeTarget.localPosition = originalPosition;

        // 현재 실행 중인 Shake 코루틴이 없음을 표시
        _shakeCoroutine = null;
    }
}