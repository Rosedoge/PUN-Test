using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {


        #region Public Properties
        [Tooltip("Pixel offset from the player target")]
        public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

        [Tooltip("UI Text to display Player's Name")]
        public Text PlayerNameText;
        GameObject PlayerUiPrefab;

        [Tooltip("UI Slider to display Player's Health")]
        public Slider PlayerHealthSlider;


        #endregion


        #region Private Properties
        float _characterControllerHeight = 0f;
        Transform _targetTransform;
        [SerializeField]
        PlayerManager _target;
        #endregion


        #region MonoBehaviour Messages
        void Awake()
        {
            this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        }

        void Start()
        {
            //PlayerUiPrefab = (GameObject)Resources.Load("Player UI");
            //if (PlayerUiPrefab != null)
            //{
            //    GameObject _uiGo = (GameObject)Instantiate(Resources.Load("Player UI"));
            //    //(GameObject)Instantiate(Resources.Load("goal"),
            //    _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            //}
            //else
            //{
            //    Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            //}
        }
        void Update()
        {
            // Reflect the Player Health
            if (PlayerHealthSlider != null)
            {
                PlayerHealthSlider.value = _target.Health;
            }
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (_target == null)
            {
                Destroy(this.gameObject);
                return;

            }
        }

        #endregion


        #region Public Methods
        void LateUpdate()
        {
            Vector3 _targetPosition;
            // #Critical
            // Follow the Target GameObject on screen.
            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;
                this.transform.position = new Vector3(0, 0, 0);// Camera.main.WorldToScreenPoint(_targetPosition);// + ScreenOffset;
            }
        }

        public void SetTarget(PlayerManager target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            _target = target;
            CharacterController _characterController = _target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (_characterController != null)
            {
                _characterControllerHeight = _characterController.height;
            }
            if (PlayerNameText != null)
            {
                PlayerNameText.text = _target.photonView.owner.name;
            }
        }

        #endregion


    }
}