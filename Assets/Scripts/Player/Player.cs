using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] public Rigidbody _rigidbody;
    [SerializeField] private GameObject effect;

    public const float accelRange = 50;

    public float rbMagnitude { get; private set; }
    public float maxSpeed = 25;
    public float acceling { get; private set; }

    private Vector3 ringForward;
    private float forcedTime, forcedDecayTime, forcedSpeedRate, viveRate, dragPlus;
    private bool forced, voluntary;

    private float oldCtrlDis = 0;

    private void Start()
    {
        Manager.player = this;
        viveRate = Manager.gameType == Manager.GameType.Vive ? 3 : 1;
    }

    void FixedUpdate()
    {
        if (Manager.result)
        {
            _rigidbody.velocity = new Vector3(1, 0, 1).normalized * maxSpeed;
            if (transform.position.z >= 3000) transform.position += Vector3.left * 6000 + Vector3.back * 6000;
            rbMagnitude = maxSpeed;
        }
        if (!Manager.gaming)
        {
            Manager.playerPos = transform.position;
            return;
        }

        //トリガーを押したままコントローラーを動かすと加速
        float sumCtrlDis = 0;
        for (int i = 0; i < InputManager.GetCtrlCount(); i++)
        {
            if (InputManager.GetTrigger(i))
            {
                _rigidbody.drag += Time.deltaTime * Mathf.Max(1, InputManager.GetCtrlCount());
                voluntary = true;
                sumCtrlDis += InputManager.GetCtrlDis(i) / InputManager.GetCtrlCount() * Time.fixedDeltaTime * viveRate;
            }
        }
        if (sumCtrlDis == 0)
        {
            _rigidbody.drag = 0.25f;
            _rigidbody.velocity += InputManager.GetHMDrot() *
                transform.rotation * Vector3.forward * sumCtrlDis * maxSpeed * 10;
        }
        else
        {
            _rigidbody.velocity += InputManager.GetHMDrot() *
                transform.rotation * Vector3.forward * Mathf.Max(0, sumCtrlDis - oldCtrlDis) * maxSpeed * 120 * viveRate;
        }
        sumCtrlDis *= 1 / Time.fixedDeltaTime;
        oldCtrlDis = sumCtrlDis;

        if (forcedTime > 0)
        {
            forcedTime -= Time.fixedDeltaTime;
            if (forcedTime <= 0) forced = false;

            //自発的に動かないと強い力で移動
            if (!voluntary) _rigidbody.velocity += ringForward * maxSpeed * forcedSpeedRate * Time.fixedDeltaTime * 10;
            //自発的に動くと強制力が弱まる
            else _rigidbody.velocity += ringForward * maxSpeed * forcedSpeedRate;

            _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed * forcedSpeedRate;

            rbMagnitude = maxSpeed * forcedSpeedRate;
        }
        else if (forcedDecayTime > 0)
        {
            forcedDecayTime -= Time.fixedDeltaTime;

            //緩やかな速度制限
            if (rbMagnitude > maxSpeed * forcedSpeedRate) _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed * forcedSpeedRate;
        }
        else
        {
            //速度制限
            rbMagnitude = _rigidbody.velocity.magnitude;
            if (rbMagnitude > maxSpeed) _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
        }
        Manager.playerPos = transform.position;
    }

    public Rigidbody GetRigidbody() { return _rigidbody; }
    public void AddNormalSpeed(float value) { maxSpeed += value; }

    public void ForcedIvent(Vector3 forward, float time, float speedRate)
    {
        ringForward = forward + _rigidbody.velocity.normalized * 0.02f;
        forcedTime = time * 0.5f;
        forcedDecayTime = time * 0.5f;
        forcedSpeedRate = speedRate;
        forced = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null) return;

        HitStop.Stop(0.1f);

        enemy.TakeDamage(rbMagnitude, 1);
        enemy.transform.position += (enemy.transform.position - (transform.position + Vector3.up * Manager.PLAYER_HEIGHT)).normalized;
        enemy._rigidbody.velocity =  (enemy.transform.position - (transform.position + Vector3.up * Manager.PLAYER_HEIGHT)).normalized * rbMagnitude * Time.fixedDeltaTime;
        _rigidbody.velocity = Vector3.zero;

        GameObject instant = Instantiate(effect, transform.position + transform.forward, transform.rotation);
        Destroy(instant, 2);
    }
}
