using System;
using System.Collections;
using System.Collections.Generic;
using ScratchCardAsset;
using Unity.VisualScripting;
using UnityEngine;

    public class CursorManager : MonoBehaviour
    {
        private SpriteRenderer cursorRenderer;
        private bool isInDeadzone = true;
        private Vector3 initialScratchPos;
        private SpriteRenderer cardSprite;
        
        // public static CursorManager instance;
        public List<Sprite> idleCursorList;
        public List<Sprite> scratchCursorList;
        public float scratchRadius = 1;
        public float initialScratchDeadzone = 0.5f;
        public float fullScratchRevealDistance = 0.2f;

        [Header("Scratch Card")]
        private bool isOverScratchCard = false;
        private bool isUIShown = false;
        public Vector2 scratchCardCenter;
        public Vector2 scratchCardSize;
        public float revealPressure = 1;
        
        private void Awake()
        {
            // instance = this;
        }

        private void OnEnable()
        {
            GameManager.Instance.UIManager.onUIShown += LockCursor;
            GameManager.Instance.UIManager.onUIHidden += UnlockCursor;
        }

        private void OnDisable()
        {
            GameManager.Instance.UIManager.onUIShown -= LockCursor;
            GameManager.Instance.UIManager.onUIHidden -= UnlockCursor;
        }

        void Start()
        {
            cursorRenderer = GetComponent<SpriteRenderer>();
            isUIShown = false;
        }

        void Update()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            
            cardSprite = GameManager.Instance.currentScratchCard.transform.Find("Scratch Surface Sprite").GetComponent<SpriteRenderer>();

            if (GameManager.Instance.currentCurseLevel > GameManager.Instance.maxCurseLevel) return;
            if (isOverScratchCard && !isUIShown)
            {
                if (Input.GetMouseButton(0)) ChangeCursor(scratchCursorList[GameManager.Instance.currentCurseLevel]);
                else ChangeCursor(idleCursorList[GameManager.Instance.currentCurseLevel]);
            }
            else
            {
                ChangeCursor(idleCursorList[GameManager.Instance.currentCurseLevel]);
            }
        }

        private void FixedUpdate()
        {
            isOverScratchCard = DetectHover();
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            cursorRenderer.transform.position = cursorPosition;

            if (Input.GetMouseButtonDown(0) && isOverScratchCard && !isUIShown)
                initialScratchPos = cursorRenderer.transform.position;
            
            if (Input.GetMouseButton(0) && isOverScratchCard && !isUIShown)
            {
                cursorRenderer.transform.position = new Vector3(initialScratchPos.x, cursorPosition.y);
                if (Input.GetAxis("Mouse Y") >= 0) return;
                if ((cursorRenderer.transform.position - initialScratchPos).magnitude >= initialScratchDeadzone) isInDeadzone = false;
                if (!isInDeadzone)
                {
                    GameManager.Instance.currentScratchCard.GetComponent<ScratchCardManager>().Card.ScratchHole(ConvertToScratchCardTexturePosition(), revealPressure);
                    DetectScratch();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isInDeadzone = true;
            }
        }

        private void DetectScratch()
        {
            Collider2D iconCollider = Physics2D.OverlapCircle(transform.position, scratchRadius, LayerMask.GetMask("Icon"));
            if (iconCollider != null)
            {
                IconBase iconScript = iconCollider.GetComponent<IconBase>();
                if (iconScript.fullScratchToReveal)
                {
                    if((iconCollider.transform.position - transform.position).magnitude <= fullScratchRevealDistance) iconScript.OnReveal();
                    // print((iconCollider.transform.position - transform.position).magnitude);
                }   
                else iconScript.OnReveal();
            }
        }

        private bool DetectHover()
        {
            if (Mathf.Abs(transform.position.x - scratchCardCenter.x) <= scratchCardSize.x / 2 &&
                Mathf.Abs(transform.position.y - scratchCardCenter.y) <= scratchCardSize.y / 2) return true;
            else return false;
        }
        
        public Vector2 ConvertToScratchCardTexturePosition()
        {
            Vector3 scratchCardOrigin = new Vector2(cardSprite.transform.position.x - cardSprite.sprite.bounds.size.x / 2,
                cardSprite.transform.position.y - cardSprite.sprite.bounds.size.y / 2);
            Vector2 relativePos = cursorRenderer.transform.position - scratchCardOrigin;
            Vector2 uvPosition = new Vector2(relativePos.x / cardSprite.sprite.bounds.size.x, relativePos.y / cardSprite.sprite.bounds.size.y);

            Vector2 convertedPosition = new Vector2(Mathf.FloorToInt(uvPosition.x * cardSprite.sprite.texture.width),
                Mathf.FloorToInt(uvPosition.y * cardSprite.sprite.texture.height));
            // print(convertedPosition);

            return convertedPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, scratchRadius); //Draw Finger Tip Overlap Circle
            Gizmos.DrawWireCube(scratchCardCenter,scratchCardSize); //Draw Scratch Card Wireframe
        }

        public void ChangeCursor(Sprite cursorSprite)
        {
            cursorRenderer.sprite = cursorSprite;
        }

        public void LockCursor()
        {
            isUIShown = true;
        }

        public void UnlockCursor()
        {
            isUIShown = false;
        }

        public void HideCursor()
        {
            cursorRenderer.enabled = false;
        }

        public void ShowCursor()
        {
            cursorRenderer.enabled = true;
        }
        
    }
