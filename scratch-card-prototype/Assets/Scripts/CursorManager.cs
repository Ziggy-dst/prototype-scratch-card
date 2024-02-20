using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CursorManager : MonoBehaviour
    {
        private SpriteRenderer cursorRenderer;
        // public static CursorManager instance;
        public Sprite defaultCursor;
        public Sprite scratchCursor;

        // private void Awake()
        // {
        //     instance = this;
        // }

        void Start()
        {
            cursorRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Cursor.visible = false;
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorRenderer.transform.position = cursorPosition;
            
            if(Input.GetMouseButtonDown(0)) ChangeCursor(scratchCursor);
            if(Input.GetMouseButtonUp(0)) ResumeCursor();
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
