using UnityEngine;


public class DynamicSplitScreen : MonoBehaviour {

	
    [SerializeField] Transform _player1;
    [SerializeField] Transform _player2;

    //This is the distance at which the split screen will be activated.
    [SerializeField] float _splitDistance = 5;

    //The color and width of the splitter.
    [SerializeField] Color _splitterColor;
    [SerializeField] float _splitterWidth;

    //The two cameras, initialized and referenced in Start()
    private GameObject _camera1;
    private GameObject _camera2;

    //The two quads used to draw the second screen
    private GameObject _split;
    private GameObject _splitter;
    
    [SerializeField] private Material _unlitMaterial;

    void Start () {
        //Reference to camera1, initalizes camera2.
        _camera1 = Camera.main.gameObject;
        Camera cam1 = _camera1.GetComponent<Camera>();
        _camera2 = new GameObject("Split Screen Camera");
        Camera cam2 = _camera2.AddComponent<Camera>();

        // Ensures that the second camera renders before the first
        cam2.depth = cam1.depth - 1;
        //Sets up the culling mask of camera2 to ignore "TransparentFX", since we don't want to render the split and splitter on both cameras.
        cam2.cullingMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));

        //Sets up the splitter and initalizes the gameobject.
        _splitter = GameObject.CreatePrimitive (PrimitiveType.Quad);
        _splitter.transform.parent = gameObject.transform;
        _splitter.transform.localPosition = Vector3.forward;
        _splitter.transform.localScale = new Vector3 (1,  _splitterWidth/10, 1f) * 3;
        _splitter.transform.localEulerAngles = Vector3.zero;
        _splitter.SetActive (false);

        //Sets up the screen split and initalizes the gameobject.
        _split = GameObject.CreatePrimitive (PrimitiveType.Quad);
        _split.transform.parent = _splitter.transform;
        _split.transform.localPosition = new Vector3(0, -(1 / (_splitterWidth / 10)), 0.0001f); // Add a little bit of Z-distance to avoid clipping with splitter
        _split.transform.localScale = new Vector3 (1, 2/(_splitterWidth/10), 1f);
        _split.transform.localEulerAngles = Vector3.zero;

        //Creates the temporary materials required to create the splitscreen effect.
        Material tempMat = _unlitMaterial; // new Material(shader); // new Material (Shader.Find ("Unlit/Color"));
        tempMat.color = _splitterColor;
        _splitter.GetComponent<Renderer>().material = tempMat;
        _splitter.GetComponent<Renderer> ().sortingOrder = 2;
        _splitter.layer = LayerMask.NameToLayer ("TransparentFX");
        
        Material tempMat2 = new Material (Shader.Find ("Mask/SplitScreen"));

        _split.GetComponent<Renderer>().material = tempMat2;
        _split.layer = LayerMask.NameToLayer ("TransparentFX");
    }

    void LateUpdate () {
        //Gets the z axis distance between the two players and the general distance.
        float zDistance = _player1.position.z - _player2.transform.position.z;
        float distance = Vector3.Distance (_player1.position, _player2.transform.position);

        //Sets the angle of the player, depending on which player leads on the x axis.
        float angle;
        if (_player1.transform.position.x <= _player2.transform.position.x) {
            angle = Mathf.Rad2Deg * Mathf.Acos (zDistance / distance);
        } else {
            angle = Mathf.Rad2Deg * Mathf.Asin (zDistance / distance) - 90;
        }

        //Rotates the splitter relative to the new angle.
        _splitter.transform.localEulerAngles = new Vector3 (0, 0, angle);

        //Gets the midpoint between player1 and player2.
        Vector3 midPoint = new Vector3 ((_player1.position.x + _player2.position.x) / 2, (_player1.position.y + _player2.position.y) / 2, (_player1.position.z + _player2.position.z) / 2); 

        //Waits for the two cameras to split, calculates a midpoint relative to the positional difference between camera1 and camera2.
        if (distance > _splitDistance) {
            Vector3 offset = midPoint - _player1.position; 
            offset.x = Mathf.Clamp(offset.x,-_splitDistance/2,_splitDistance/2);
            offset.y = Mathf.Clamp(offset.y,-_splitDistance/2,_splitDistance/2);
            offset.z = Mathf.Clamp(offset.z,-_splitDistance/2,_splitDistance/2);
            midPoint = _player1.position + offset;

            Vector3 offset2 = midPoint - _player2.position; 
            offset2.x = Mathf.Clamp(offset.x,-_splitDistance/2,_splitDistance/2);
            offset2.y = Mathf.Clamp(offset.y,-_splitDistance/2,_splitDistance/2);
            offset2.z = Mathf.Clamp(offset.z,-_splitDistance/2,_splitDistance/2);
            Vector3 midPoint2 = _player2.position - offset;

            //Sets splitter + camera to active, sets the second camera position to prevent the possibility of lerping continuity errors.
            if (_splitter.activeSelf == false) {
                _splitter.SetActive (true);
                _camera2.SetActive (true);

                _camera2.transform.position = _camera1.transform.position;
                _camera2.transform.rotation = _camera1.transform.rotation;

            } else {
                //Lerps the second camera position and rotation to that of the second midpoint, i.e relative to the second player.
                _camera2.transform.position = Vector3.Lerp(_camera2.transform.position,midPoint2 + new Vector3(0,8,-4),Time.deltaTime*5);
                Quaternion newRot2 = Quaternion.LookRotation(midPoint2-_camera2.transform.position);
                _camera2.transform.rotation = Quaternion.Lerp(_camera2.transform.rotation, newRot2, Time.deltaTime*5);
            }

        } else {
            //Deactivates the splitter + camera when the distance is less than the splitting distance.
            if (_splitter.activeSelf)
                _splitter.SetActive (false);
            _camera2.SetActive (false);
        }

        /* We lerp the first camera's position and rotation to that of the second midpoint, so relative to the first player
        or when both players are in view the camera will lerp to their midpoint.*/
        _camera1.transform.position = Vector3.Lerp(_camera1.transform.position,midPoint + new Vector3(0,8,-4),Time.deltaTime*5);
        Quaternion newRot = Quaternion.LookRotation(midPoint-_camera1.transform.position);
        _camera1.transform.rotation = Quaternion.Lerp(_camera1.transform.rotation, newRot, Time.deltaTime*5);
    }
}