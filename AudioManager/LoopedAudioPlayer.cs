using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace DuszaCompetitionApplication.AudioManager
{
    public static class AudioManager {
        private static float totalVolume = 0.5f;
        public static float? TotalVolume
        {
            get { return totalVolume; }
            set
            {
                if (value != null) totalVolume = (float)value;
            }
        }

        public static async Task PlayAudio(string path)
        {
            await Task.Run(() =>
            {
                using AudioFileReader audioReader = new(path);
                using WaveOutEvent outputDevice = new WaveOutEvent { Volume = totalVolume };

                outputDevice.Init(audioReader);
                outputDevice.Play();

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                    Task.Delay(100).Wait();
            });
        }


        private static Dictionary<string, AudioPlayerInstance> playingLoopedAudioFile = new();

        public static void PlayAndLoopAudio(string path, bool forceCreateNewOne = false)
        {
            if (forceCreateNewOne)
            {
                
            }
            else
            {
                if (playingLoopedAudioFile.Keys.Contains(path)) return;

            }

            //     if (lastLoopedAudioPath == path) return;

            //     if (musicOutputDevice != null)
            //     {
            //         loopStream?.Dispose();
            //         musicOutputDevice?.Dispose();
            //     }
            //     loopStream = new();
            //     musicOutputDevice = new();
            //     lastLoopedAudioPath = path;
            //     musicOutputDevice.Init(loopStream);
            //     musicOutputDevice.Play();
        }
    }

    public class AudioPlayerInstance
    {
        public LoopStream loopStream;
        public WaveOutEvent musicOutputDevice;

        public AudioPlayerInstance(string path)
        {
            loopStream = new(new AudioFileReader(path));
            musicOutputDevice = new();
        }

    }

    public class LoopStream : WaveStream
    {
        // https://markheath.net/post/looped-playback-in-net-with-naudio
        WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        public bool EnableLooping { get; set; }
        public override WaveFormat WaveFormat { get { return sourceStream.WaveFormat; }}

        public override long Length { get { return sourceStream.Length; }}

        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        break;
                    }
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}

