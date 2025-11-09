using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace DuszaCompetitionApplication.Audio
{
    public static class AudioManager
    {
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

        private static WaveOutEvent? currentOutputDevice;
        private static AudioFileReader? currentAudioReader;

        public static void PlayAudio(string path)
        {
            try
            {
                if (currentOutputDevice is not null)
                {
                    try
                    {
                        currentOutputDevice.Stop();
                    }
                    catch {} //🥀

                    currentAudioReader?.Dispose();
                    currentOutputDevice.Dispose();

                    currentOutputDevice = null;
                    currentAudioReader = null;
                }

                var outputDevice = new WaveOutEvent { Volume = totalVolume };
                var audioReader = new AudioFileReader(path);

                currentOutputDevice = outputDevice;
                currentAudioReader = audioReader;

                outputDevice.Init(audioReader);
                outputDevice.Play();


                outputDevice.PlaybackStopped += (s, e) =>
                {
                    audioReader.Dispose();
                    outputDevice.Dispose();

                    if (ReferenceEquals(outputDevice, currentOutputDevice))
                    {
                        currentOutputDevice = null;
                        currentAudioReader = null;
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio playback failed: {ex.Message}");
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
            try
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
                    case SoundEffectTypes.win:
                        PlayAudio(soundEffectFolder + $"fight/sound_win.wav");
                        break;
                    case SoundEffectTypes.lose:
                        PlayAudio(soundEffectFolder + $"fight/sound_lose.wav");
                        break;
                }
            } catch(Exception)
            {
                Console.WriteLine("Audio wanted to crash the whole system, but i saved it");
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
        levelup,
        win,
        lose
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

