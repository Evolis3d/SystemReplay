using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.IO;
using UnityEngine.Profiling;


namespace  evolis3d.SystemReplay
{
    public enum ReplayModeEnum
    {
        Recording,
        Playing
    }

    public class ReplayData : MonoBehaviour
    {
        #region memorystream
            private MemoryStream memoryStream = null;
            private BinaryWriter binaryWriter = null;
            private BinaryReader binaryReader = null;
        #endregion
        
        private Dictionary<string, Transform> Actors;

        private ReplayModeEnum _replayMode;
        public ReplayModeEnum ReplayMode
        {
            get => _replayMode;
            set
            {
                _replayMode = value;

                if (value == ReplayModeEnum.Recording)
                {
                    InitRecording();

                }
                else if (value == ReplayModeEnum.Playing)
                {
                    InitReplaying();
                } 
            }
        }

        #region notifications & events
            public delegate void NotifyRecordingStarted();
            public delegate void NotifyRecordingStopped();
            public delegate void NotifyPlaybackStarted();
            public delegate void NotifyPlaybackStopped();
            public delegate void NotifyPlaybackPaused();
            public delegate void NotifyPlaybackResume();

            public event NotifyRecordingStarted RecordingStarted;
            public event NotifyRecordingStopped RecordingStopped;
            public event NotifyPlaybackStarted PlaybackStarted;
            public event NotifyPlaybackStopped PlaybackStopped;
            public event NotifyPlaybackPaused PlaybackPaused;
            public event NotifyPlaybackResume PlaybackResume;
        #endregion

        #region internal functions

        private void Awake()
        {
            Actors = new Dictionary<string, Transform>();
            
            //no sé si ésto va aquí...
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);
        }

        /// <summary>
        /// Setups memoryStream for recording, resets mem position to 0, at the beginning.
        /// </summary>
        private void InitRecording()
        {
            memoryStream.SetLength(0);
            memoryStream.Seek(0, SeekOrigin.Begin);
            binaryWriter.Seek(0, SeekOrigin.Begin);            
        }

        /// <summary>
        /// TODO: Setups memoryStream for playback the recorded data.
        /// </summary>
        private void InitReplaying()
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
        }
        
        /// <summary>
        /// Writes the transform on the memoryStream, I think...
        /// </summary>
        /// <param name="transform"></param>
        private void SaveTransform(string tagname, Transform transform)
        {
            binaryWriter.Write(tagname);
            binaryWriter.Write(transform.localPosition.x);
            binaryWriter.Write(transform.localPosition.y);
            binaryWriter.Write(transform.localPosition.z);
        }

        /// <summary>
        /// AQUI VA LO GORDO!! Checks the current ReplayMode and records or playbacks to/from the memoryStream.
        /// </summary>
        private void FixedUpdate()
        {
            if (ReplayMode == ReplayModeEnum.Recording)
            {
                foreach (var item in Actors)
                {
                   SaveTransform(item.Key, item.Value); 
                }
            }
            else if(ReplayMode == ReplayModeEnum.Playing)
            {
                //asignar y reproducir cada uno...
            }
        }


        private void TrimList()
            {
                //TODO: limpiar items de la lista que tengan muy poca diferencia en valores.
            }
        
       
        #endregion
        
        

        #region API & public functions
            
            /// <summary>
            /// Returns the transform of the item, if it was stored in the list. Otherwise, returns null. 
            /// </summary>
            /// <param name="tagname"></param>
            /// <returns></returns>
            [CanBeNull]
            public Transform GetFromList(string tagname)
            {
                Transform temp;
                
                if (Actors.Count == 0) return null;
                
                var isTemp = Actors.TryGetValue(tagname, out temp);
               
                return temp;
            }
            
            /// <summary>
            /// Stores a Transform to the internal list, for later use in the Replay System.
            /// You may want to store an item based on its Tag or GameObject's name...
            /// </summary>
            /// <param name="tagname"></param>
            /// <param name="transformData"></param>
            public void AddToList(string tagname, Transform transformData)
            {
                if (string.IsNullOrEmpty(tagname) || transformData == null) return;
                
                Actors.Add(tagname,transformData);
            }
            
        #endregion
       

        

    }
}
