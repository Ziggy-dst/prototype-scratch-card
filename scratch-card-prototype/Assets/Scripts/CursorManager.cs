using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    public class CursorManager : MonoBehaviour
    {
        private SpriteRenderer cursorRenderer;
        public static CursorManager instance;
        public Sprite defaultCursor;
        public Sprite scratchCursor;
        public float scratchRadius = 1;

        [Header("Scratch Card")]
        private bool isOverScratchCard = false;
        public Vector2 scratchCardCenter;
        public Vector2 scratchCardSize;
        
        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            cursorRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Cursor.visible = false;
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorRenderer.transform.position = cursorPosition;
            isOverScratchCard = DetectHover();

            if (isOverScratchCard)
            {
                if (Input.GetMouseButton(0)) ChangeCursor(scratchCursor);
                if (Input.GetMouseButtonUp(0)) ResumeCursor();
            }
            else ResumeCursor();
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0)) DetectScratch();
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
                    print((iconCollider.transform.position - transform.position).magnitude);
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
        
        public void ResumeCursor()
        {
            cursorRenderer.sprite = defaultCursor;
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
