//-----------------------------------------------------------------------
// <copyright file="CameraPointer.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using TMPro;
/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>
public class CameraPointer : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;
    public float sensitivity=0.5f;
    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;
    public TMP_Text textbox;
    public TMP_Text textboxTime;

    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    int ct = 0;
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed
        // at.

        // rotation.y += Input.GetAxis("Mouse X");
        // rotation.x += -Input.GetAxis("Mouse Y");
        // transform.eulerAngles = (Vector2)rotation * sensitivity;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnPointerExit");
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnter");
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            _gazedAtObject?.SendMessage("OnPointerExit");
            _gazedAtObject = null;
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
            ct++;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            _gazedAtObject?.SendMessage("OnPointerClick");
            ct++;
        }

        textbox.text = "Score: " + ct;
        textboxTime.text = "Time: "+Time.time;
    }
}