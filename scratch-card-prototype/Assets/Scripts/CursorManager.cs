using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    public class CursorManager : MonoBehaviour
    {
        private SpriteRenderer cursorRenderer;
        // public static CursorManager instance;
        public List<Sprite> idleCursorList;
        public List<Sprite> scratchCursorList;
        public float scratchRadius = 1;

        [Header("Scratch Card")]
        private bool isOverScratchCard = false;
        private bool isUIShown = false;
        public Vector2 scratchCardCenter;
        public Vector2 scratchCardSize;
        
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
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorRenderer.transform.position = cursorPosition;
            isOverScratchCard = DetectHover();

            if (isOverScratchCard && !isUIShown)
            {
                if (Input.GetMouseButton(0)) ChangeCursor(scratchCursorList[GameManager.Instance.currentCurseLevel]);
                else ChangeCursor(idleCursorList[GameManager.Instance.currentCurseLevel]);
            }
            else
            {
                if (GameManager.Instance.currentCurseLevel > GameManager.Instance.maxCurseLevel) return;
                ChangeCursor(idleCursorList[GameManager.Instance.currentCurseLevel]);
            }
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0) && isOverScratchCard && !isUIShown) DetectScratch();
        }

        private void DetectScratch()
        {
            Collider2D iconCollider = Physics2D.OverlapCircle(transform.position, scratchRadius, LayerMask.GetMask("Icon"));
            if (iconCollider != null)
            {
                IconBase iconScript = iconCollider.GetComponent<IconBase>();
                if (iconScript.fullScratchToReveal)
                {
                    if((iconCollider.transform.position - transform.position).magnitude <= 0.15f) iconScript.OnReveal();
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
