using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitStop : MonoBehaviour
{
    // 현재 실행 중인 HitStop 코루틴을 저장
    // 연속으로 HitStop이 호출될 경우 기존 코루틴을 중지하기 위해 사용
    private Coroutine _hitStopCoroutine;

    [SerializeField] private Slider _hitStopSlider;
    // HitStop이 시작되기 전의 Time.timeScale 값을 저장
    // HitStop 종료 후 원래 속도로 복구하기 위해 사용
    private float _originalTimeScale = 1f;

    // 외부에서 HitStop을 실행할 때 호출하는 메서드
    // duration: 게임을 멈출 실제 시간
    public void Play(float duration)
    {
        // 이미 HitStop이 실행 중이라면
        if (_hitStopCoroutine != null)
        {
            // 기존 HitStop 코루틴을 중지
            StopCoroutine(_hitStopCoroutine);
        }
        else
        {
            // HitStop이 처음 시작되는 경우
            // 현재 timeScale을 저장
            _originalTimeScale = Time.timeScale;
        }

        // 새로운 HitStop 코루틴 시작
        _hitStopCoroutine = StartCoroutine(HitStopCoroutine(duration));
    }

    // 실제로 게임 시간을 멈추고 다시 복구하는 코루틴
    private IEnumerator HitStopCoroutine(float duration)
    {
        // 게임 내 시간 흐름을 정지
        Time.timeScale = _hitStopSlider.value;

        // timeScale의 영향을 받지 않는 실제 시간 기준으로 대기
        // WaitForSeconds를 사용하면 timeScale이 0이기 때문에
        // 대기 시간이 진행되지 않을 수 있음
        yield return new WaitForSecondsRealtime(duration);

        // HitStop이 끝나면 기존 timeScale로 복구
        Time.timeScale = _originalTimeScale;

        // 현재 실행 중인 HitStop 코루틴이 없음을 표시
        _hitStopCoroutine = null;
    }
}