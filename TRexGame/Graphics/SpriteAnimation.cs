using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRexGame.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        // Indexer
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);
            }
        }

        public int FrameCount => _frames.Count;

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                // we have PlaybackProgress and all frames have timestamp. We should be able to tell which frame should be drawn
                // we check whats the current timeprogress timestamp and check which frames are <= than our current timestamp and then 
                // select the frame with the highest timestamp
                return _frames
                    .Where(f => f.TimeStamp <= PlaybackProgress)
                    .OrderBy(f => f.TimeStamp)
                    .LastOrDefault();
            }
        }
        /// <summary>
        /// We return the timestamp of the frame that has the maximum timeframe in all the frames list. 
        /// </summary>
        public float Duration
        {
            get
            {
                if (!_frames.Any())
                    return 0;
                return _frames.Max(f => f.TimeStamp);
            }
        }

        public bool IsPlaying { get; private set; }

        public float PlaybackProgress { get; private set; }

        // by default it will be true
        public bool ShouldLoop { get; set; } = true;

        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
            _frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                // we will get total number of seconds that passed within this frame and increase progress
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Here we check how much time has passed since the start of animation and see if it is more than our set duration of the animation
                // 'Duration' variable simply gets the last frames' timestamp. 
                // If Progress is > Duration, then we need to concider either looping the animation or stopping it
                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop();
                }
                
            }

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;

            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);
        }

        public void Play()
        {
            IsPlaying = true;
            
        }

        public void Stop()
        {
            IsPlaying = false;
            // When we stop animation we need to set playback progress to 0
            // actually we want animation to stop at the last frame so don't set to 0
            //PlaybackProgress = 0;
        }

        public void ResetPlaybackProgress()
        {
            PlaybackProgress = 0;
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "A frame wiht index " + index + " does not exist in this animation.");

            return _frames[index];    
        }   
        // Clear frames from animation to clear garbage
        public void Clear()
        {

            Stop();
            _frames.Clear();

        }

        // helper method to create an animation
        public static SpriteAnimation CreateSimpleAnimation(Texture2D texture, Point startPos, int width, int height, Point offset, int frameCount, float frameLength)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            SpriteAnimation anim = new SpriteAnimation();

            for (int i = 0; i < frameCount; i++)
            {
                // for each iteration add a sprite to the animation
                Sprite sprite = new Sprite(texture, startPos.X + i * offset.X, startPos.Y + i * offset.Y, width, height);
                anim.AddFrame(sprite, frameLength * i);

                // last iteration? add dummy frame to indicate animation end
                // we add this last frame to "stretch" the previous frame otherwise the previous frame would just lsat split second and we will not see it
                if (i == frameCount - 1)
                    anim.AddFrame(sprite, frameLength * (i + 1));

            }

            return anim;

        }

    }
}
