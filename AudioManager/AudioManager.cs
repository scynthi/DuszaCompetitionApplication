using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace DuszaCompetitionApplication.Audio
{
    public static class AudioManager
    {
        private static int maxSfxAudioPlayer = 32;
        private static int currentSfxAudioPlayer = 0;
        private static Dictionary<string, AudioPlayerInstance> playingLoopedAudioPlayers = new();
        private static Random random = new();

        private static string soundEffectFolder = "./Assets/Audio/SoundEffects/";
        private static float totalVolume = 0.5f;
        public static float? TotalVolume
        {
            get { return totalVolume; }
            set
            {
                if (value != null) totalVolume = (float)value;

                foreach (AudioPlayerInstance audio in playingLoopedAudioPlayers.Values)
                {
                    audio.SetVolume(totalVolume);
                }
            }
        }

        public static async void PlayAudio(string path)
        {
            try
            {
                // Console.WriteLine($"{maxSfxAudioPlayer}/{currentSfxAudioPlayer}");

                if (currentSfxAudioPlayer >= maxSfxAudioPlayer) return;
                currentSfxAudioPlayer++;

                await Task.Run(() =>
                {
                    using AudioFileReader audioReader = new(path);
                    using WaveOutEvent outputDevice = new WaveOutEvent { Volume = totalVolume };

                    outputDevice.Init(audioReader);
                    outputDevice.Volume = totalVolume;
                    outputDevice.Play();

                    while (outputDevice != null)
                    {
                        Task.Delay(100).Wait();
                        
                        if (outputDevice.PlaybackState == PlaybackState.Stopped)
                        {
                            audioReader.Dispose();
                            outputDevice.Dispose();
                            currentSfxAudioPlayer--;
                            break;
                        }
                    }
                });
            } catch(Exception)
            {
                Console.WriteLine("Audio wanted to crash the whole system, but i saved it");
            }

        }

        public static void PlayAndLoopAudio(string path)
        {
            if (FindAudioPlayer(path))
            {
                AudioPlayerInstance existingInstance = playingLoopedAudioPlayers[path];
                existingInstance.Resume();
                return;
            }


            AudioPlayerInstance audioPlayerInstance = new(path);
            audioPlayerInstance.SetVolume(totalVolume);
            audioPlayerInstance.Play();

            playingLoopedAudioPlayers.Add(path, audioPlayerInstance);
        }

        public static void FindAndDestroyAudio(string path)
        {
            if (FindAudioPlayer(path))
            {
                AudioPlayerInstance existingInstance = playingLoopedAudioPlayers[path];
                existingInstance.Destroy();
            }
        }

        public static void FindAndPauseAudio(string path)
        {
            if (FindAudioPlayer(path))
            {
                AudioPlayerInstance existingInstance = playingLoopedAudioPlayers[path];
                existingInstance.Pause();
            }
        }

        public static void PlaySoundEffect(SoundEffectTypes type)
        {
            switch (type)
            {
                case SoundEffectTypes.attack:
                    PlayAudio(soundEffectFolder + $"attack/attack ({random.Next(1, 3)}).wav");
                    break;
                case SoundEffectTypes.click:
                    PlayAudio(soundEffectFolder + $"click/wood_click ({random.Next(1, 8)}).wav");
                    break;
                case SoundEffectTypes.death:
                    PlayAudio(soundEffectFolder + $"death/sound_death.wav");
                    break;
                case SoundEffectTypes.draw:
                    PlayAudio(soundEffectFolder + $"draw/sound_draw.wav");
                    break;
                case SoundEffectTypes.hover:
                    PlayAudio(soundEffectFolder + $"hover/wood_hover ({random.Next(1,4)}).wav");
                    break;
                case SoundEffectTypes.levelup:
                    PlayAudio(soundEffectFolder + $"levelup/sound_level_up.wav");
                    break;
            }
        }

        public static bool FindAudioPlayer(string path) => playingLoopedAudioPlayers.Keys.Contains(path);
    }

    public enum SoundEffectTypes
    {
        click,
        hover,
        death,
        draw,
        attack,
        levelup
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

        public void Play()
        {
            musicOutputDevice.Init(loopStream);
            musicOutputDevice.Play();
        }

        public void Destroy()
        {
            musicOutputDevice.Dispose();
            loopStream.Dispose();
        }

        public void Resume()
        {
            musicOutputDevice.Play();
        }

        public void SetVolume(float value = 1)
        {
            musicOutputDevice.Volume = value;
        }

        public void Pause()
        {
            musicOutputDevice.Pause();
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

