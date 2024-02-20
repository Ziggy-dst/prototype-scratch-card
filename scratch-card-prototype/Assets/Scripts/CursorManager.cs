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
        public Vector2 scratchCardWidth;

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
            Collider2D iconCollider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), scratchRadius, LayerMask.NameToLayer("Icon"));
            print(iconCollider);
            if (iconCollider != null) iconCollider.GetComponent<IconBase>().OnReveal();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), scratchRadius);
            Gizmos.DrawWireCube();
            Gizmos.draw
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
