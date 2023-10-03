using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace AliScripts
{
    public class AliExtras : MonoBehaviour
    {
        public static void DestroyChildren(GameObject targetObject)
        {
            int childCount = targetObject.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = targetObject.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public static bool GetBoolFromString(string inputString)
        {
            try
            {
                bool parsedBool = bool.Parse(inputString);
                Debug.Log("Parsed bool using bool.Parse(): " + parsedBool);

                return parsedBool;
            }
            catch (System.Exception)
            {
                Debug.LogError("Failed to parse using bool.Parse()");
                return false;
            }
        }

        public static bool CheckInternetConnection()
        {
            try
            {
                // Create a TCP client to google.com on port 80 (HTTP).
                TcpClient client = new TcpClient("www.google.com", 80);

                // Close the TCP client after successfully connecting.
                client.Close();

                return true; // Internet connection is available.
            }
            catch (SocketException e)
            {
                Debug.LogError("No Internet Connection: Reason: " + e.Message);
                return false; // No internet connection.
            }
        }
    }
}