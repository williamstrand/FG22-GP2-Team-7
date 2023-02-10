using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Liquid : MonoBehaviour
{
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;

    [SerializeField] float _maxWobble = 0.03f;
    [SerializeField] float _wobbleSpeedMove = 1f;
    [SerializeField] float _fillAmount = 0.5f;
    [SerializeField] float _recovery = 1f;
    [SerializeField] float _thickness = 1f;
    
    [Range(0, 1)]
    public float compensateShapeAmount;
    
    [SerializeField] Mesh _mesh;
    [SerializeField] Renderer _renderer;
    
    private Vector3 _position;
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private Quaternion _lastRotation;
    private Vector3 _angularVelocity;
    private float _wobbleAmountX;
    private float _wobbleAmountZ;
    private float _wobbleAmountToAddX;
    private float _wobbleAmountToAddZ;
    private float _pulse;
    private float _sinewave;
    private float _time = 0.5f;
    private Vector3 comp;

    // Use this for initialization
    void Start()
    {
        GetMeshAndRend();
    }

    private void OnValidate()
    {
        GetMeshAndRend();
    }

    void GetMeshAndRend()
    {
        if (_mesh == null)
        {
            _mesh = GetComponent<MeshFilter>().sharedMesh;
        }
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
    }
    void Update()
    {
        float deltaTime = 0;
        switch (updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;

            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        _time += deltaTime;

        if (deltaTime != 0)
        {


            // decrease wobble over time
            _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, (deltaTime * _recovery));
            _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, (deltaTime * _recovery));



            // make a sine wave of the decreasing wobble
            _pulse = 2 * Mathf.PI * _wobbleSpeedMove;
            _sinewave = Mathf.Lerp(_sinewave, Mathf.Sin(_pulse * _time), deltaTime * Mathf.Clamp(_velocity.magnitude + _angularVelocity.magnitude, _thickness, 10));

            _wobbleAmountX = _wobbleAmountToAddX * _sinewave;
            _wobbleAmountZ = _wobbleAmountToAddZ * _sinewave;



            // velocity
            _velocity = (_lastPosition - transform.position) / deltaTime;

            _angularVelocity = GetAngularVelocity(_lastRotation, transform.rotation);

            // add clamped velocity to wobble
            _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + (_velocity.y * 0.2f) + _angularVelocity.z + _angularVelocity.y) * _maxWobble, -_maxWobble, _maxWobble);
            _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + (_velocity.y * 0.2f) + _angularVelocity.x + _angularVelocity.y) * _maxWobble, -_maxWobble, _maxWobble);
        }

        // send it to the shader
        _renderer.sharedMaterial.SetFloat("_WobbleX", _wobbleAmountX);
        _renderer.sharedMaterial.SetFloat("_WobbleZ", _wobbleAmountZ);

        // set fill amount
        UpdatePos(deltaTime);

        // keep last position
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    void UpdatePos(float deltaTime)
    {

        Vector3 worldPos = transform.TransformPoint(new Vector3(_mesh.bounds.center.x, _mesh.bounds.center.y, _mesh.bounds.center.z));
        if (compensateShapeAmount > 0)
        {
            // only lerp if not paused/normal update
            if (deltaTime != 0)
            {
                comp = Vector3.Lerp(comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            }
            else
            {
                comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));
            }

            _position = worldPos - transform.position - new Vector3(0, _fillAmount - (comp.y * compensateShapeAmount), 0);
        }
        else
        {
            _position = worldPos - transform.position - new Vector3(0, _fillAmount, 0);
        }
        _renderer.sharedMaterial.SetVector("_FillAmount", _position);
    }

    //https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/#post-4302796
    Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);

        if (float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }
        return angularVelocity;
    }

    float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3 lowestVert = Vector3.zero;
        Vector3[] vertices = _mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 position = transform.TransformPoint(vertices[i]);

            if (position.y < lowestY)
            {
                lowestY = position.y;
                lowestVert = position;
            }
        }
        return lowestVert.y;
    }
}

