using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR.Awareness.Semantics;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.Utilities.Input.Legacy;
using TMPro;

public class SemanticsQueryManager : MonoBehaviour
{
    private ISemanticBuffer _currentSemanticBuffer;

    public ARSemanticSegmentationManager _semanticSegmentationManager;
    public Camera _arCamera;
    public TMP_Text LOGGER;

    private void Start()
    {
        _semanticSegmentationManager.SemanticBufferUpdated += OnSemanticBufferUpdated;
    }

    private void OnSemanticBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        // Get The Current Awareness Buffer
        _currentSemanticBuffer = args.Sender.AwarenessBuffer;
    }

    private void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) return;

        var touch = PlatformAgnosticInput.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            Debug.Log($"Number of Channels: {_semanticSegmentationManager.SemanticBufferProcessor.ChannelCount}");

            foreach (var channels in _semanticSegmentationManager.SemanticBufferProcessor.Channels)
            {
                Debug.Log(channels);
            }

            int x = (int)touch.position.x;
            int y = (int)touch.position.y;

            int[] channelsIndicesInPixel = _semanticSegmentationManager.SemanticBufferProcessor.GetChannelIndicesAt(x, y);
            foreach(var indices in channelsIndicesInPixel)
            {
                Debug.Log(indices);
            }

            string[] channelsNamesInPixel = _semanticSegmentationManager.SemanticBufferProcessor.GetChannelNamesAt(x, y);
            foreach (var names in channelsNamesInPixel)
            {
                if (LOGGER.text == "[LOGGER]") LOGGER.text = "";
                LOGGER.text += $"Tapped On {names}\n";
                Debug.Log(names);
            }
        }
    }


}
