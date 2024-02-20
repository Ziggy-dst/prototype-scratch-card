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

            if (Input.GetMouseButtonDown(0)) ChangeCursor(scratchCursor);
            if (Input.GetMouseButtonUp(0)) ResumeCursor();
            
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0)) DetectScratch();
        }

        private void DetectScratch()
        {
            Collider2D iconCollider = Physics2D.OverlapCircle(transform.position, scratchRadius, LayerMask.GetMask("Icon"));
            print(iconCollider);
            if (iconCollider != null) iconCollider.GetComponent<IconBase>().OnReveal();
        }

        private bool DetectHover()
        {

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, scratchRadius);
            Gizmos.DrawWireCube(scratchCardCenter,scratchCardSize);
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
