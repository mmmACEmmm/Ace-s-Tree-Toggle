using BepInEx;
using UnityEngine;
using System.Collections.Generic;

namespace AcesTreeToggle
{
    [BepInPlugin("com.ace.treetoggle", "Ace's Tree Toggle", "1.0.0")]
    public class AcesTreeTogglePlugin : BaseUnityPlugin
    {
        private bool showGUI = true;
        private int[] treeInstanceIds = { 38314, 38662, 39636 };
        private Dictionary<int, GameObject> treeObjects = new Dictionary<int, GameObject>();
        private bool allTreesDisabled = false;

        void Start()
        {
            Logger.LogInfo($"Ace's Tree Toggle Plugin is loaded!");
            FindAndStoreTrees();
        }

        void FindAndStoreTrees()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (int id in treeInstanceIds)
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.GetInstanceID() == id)
                    {
                        treeObjects[id] = obj;
                        break;
                    }
                }
                if (!treeObjects.ContainsKey(id))
                {
                    Debug.LogWarning($"Could not find tree with Instance ID: {id}");
                }
            }
        }

        void OnGUI()
        {
            if (showGUI)
            {
                // Set up colored GUI
                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                GUI.contentColor = Color.white;

                // Main window
                GUI.Box(new Rect(40, 40, 250, 180), "Ace's Tree Toggle");

                // Close button 'X'
                GUI.backgroundColor = Color.red;
                if (GUI.Button(new Rect(260, 45, 20, 20), "X"))
                {
                    showGUI = false;
                }

                // Toggle button
                GUI.backgroundColor = allTreesDisabled ? Color.green : Color.red;
                if (GUI.Button(new Rect(50, 80, 230, 40), allTreesDisabled ? "Enable All Trees" : "Disable All Trees"))
                {
                    ToggleAllTrees(!allTreesDisabled);
                }

                // Status label
                GUI.contentColor = allTreesDisabled ? Color.red : Color.green;
                GUI.Label(new Rect(50, 130, 230, 20), $"Trees are currently: {(allTreesDisabled ? "Disabled" : "Enabled")}");

                // Reset colors
                GUI.backgroundColor = Color.white;
                GUI.contentColor = Color.white;
            }
            else
            {
                GUI.contentColor = Color.white;
                GUI.Label(new Rect(10, 10, 200, 20), "CTRL + C to open Ace's Tree Toggle");
                Event e = Event.current;
                if (e != null && e.isKey && e.keyCode == KeyCode.C && e.control)
                {
                    showGUI = true;
                }
            }
        }

        private void ToggleAllTrees(bool disable)
        {
            allTreesDisabled = disable;
            foreach (var kvp in treeObjects)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.SetActive(!disable);
                    Debug.Log($"{(disable ? "Disabled" : "Enabled")} tree with Instance ID: {kvp.Key}");
                }
                else
                {
                    Debug.LogWarning($"Tree with Instance ID: {kvp.Key} is null");
                }
            }

            Debug.Log($"All trees are now {(disable ? "disabled" : "enabled")}");
        }
    }
}