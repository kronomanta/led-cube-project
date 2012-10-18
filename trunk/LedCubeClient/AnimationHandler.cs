﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using FrameDescriptorLib;

namespace LedCubeClient
{
    class AnimationHandler
    {
        public delegate void CurrentFrameChangedDel(int frame);
        public event CurrentFrameChangedDel CurrentFrameChanged;

        readonly List<int[]> frames = new List<int[]>();
        public bool IsRunning { get; private set; }
        private int currentFrame = -1;

        public int AllFrame { get { return frames.Count; } }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                if (currentFrame == value) return;
                currentFrame = value;
                if (CurrentFrameChanged != null) 
                    CurrentFrameChanged(CurrentFrame);
            }
        }
        
        public int[] GetCurrentFrame()
        {
            return frames[currentFrame];
        }

        public bool Repeat { get; set; }

        public void LoadFrames(string path)
        {
            if (!File.Exists(path)) return;
            try
            {
                var frameCont = MyXmlSerializer.Deserialize<FrameDescriptorContainer>(path);
                if (frameCont == null) return;

                foreach (var item in frameCont.Frames)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        frames.Add(item.Frame);
                    }
                }

                CurrentFrame = 0;

            }
            catch (SerializationException e){
                ClientLogger.WriteLine(string.Format("SerializationException: {0}", e.Message));
            } catch(Exception e){
                ClientLogger.WriteLine(string.Format("Exception: {0}", e.Message));
            }
        }

        public void FristFrame()
        {
            CurrentFrame = 0;
        }

        public void PrevFrame()
        {
            if (CurrentFrame > 0) CurrentFrame--;
        }

        public void NextFrame()
        {
            if (CurrentFrame < frames.Count - 1) CurrentFrame++;
            else if (Repeat) CurrentFrame = 0;
            else IsRunning = false;
        }

        public void LastFrame()
        {
            CurrentFrame = frames.Count - 1;
        }

        public void Play()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}