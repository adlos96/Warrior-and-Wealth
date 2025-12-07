using NAudio.Wave;

namespace CriptoGame_Online.Strumenti
{
    /// <summary>
    /// Gestisce la musica di sottofondo con fade, loop e playlist
    /// </summary>
    internal class MusicManager
    {
        private static AudioFileReader? audioFile;
        private static WaveOutEvent? outputDevice;
        private static bool loop;
        private static string? currentTrack;
        private static float globalVolume = 0.7f;
        private static List<string>? currentPlaylist;
        private static int currentTrackIndex = 0;
        private static Random random = new Random();
        private static bool isStopping = false;

        /// Riproduce un file audio una sola volta
        public static void Play(string file)
        {
            if (currentTrack == file && outputDevice?.PlaybackState == PlaybackState.Playing)
                return;

            Stop();
            try
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"File non trovato: {file}");
                    return;
                }

                audioFile = new AudioFileReader(file);
                audioFile.Volume = globalVolume;
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.PlaybackStopped += OnPlaybackStopped;
                loop = false;
                currentTrack = file;
                currentPlaylist = null;
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore riproduzione musica: {ex.Message}");
            }
        }

        /// Riproduce un file audio in loop continuo
        public static void PlayLoop(string file)
        {
            if (currentTrack == file && outputDevice?.PlaybackState == PlaybackState.Playing)
                return;

            Stop();
            try
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"File non trovato: {file}");
                    return;
                }

                audioFile = new AudioFileReader(file);
                audioFile.Volume = globalVolume;
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.PlaybackStopped += OnPlaybackStopped;
                loop = true;
                currentTrack = file;
                currentPlaylist = null;
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore riproduzione musica loop: {ex.Message}");
            }
        }

        /// Riproduce una playlist di tracce in ordine casuale (ASYNC per non bloccare UI)
        public static void PlayPlaylist(List<string> playlist, bool shuffle = true)
        {
            if (playlist == null || playlist.Count == 0)
            {
                Console.WriteLine("Playlist vuota");
                return;
            }

            // Avvia in background per non bloccare l'UI
            Task.Run(() =>
            {
                try
                {
                    // Filtra solo file esistenti
                    var validTracks = playlist.Where(File.Exists).ToList();
                    if (validTracks.Count == 0)
                    {
                        Console.WriteLine("Nessuna traccia valida nella playlist");
                        return;
                    }

                    currentPlaylist = shuffle ? validTracks.OrderBy(x => random.Next()).ToList() : validTracks;
                    currentTrackIndex = 0;
                    loop = true;

                    PlayTrackFromPlaylist();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore avvio playlist: {ex.Message}");
                }
            });
        }

        private static void PlayTrackFromPlaylist()
        {
            if (currentPlaylist == null || currentPlaylist.Count == 0)
                return;

            if (currentTrackIndex >= currentPlaylist.Count)
                currentTrackIndex = 0;

            var track = currentPlaylist[currentTrackIndex];
            Stop();

            try
            {
                audioFile = new AudioFileReader(track);
                audioFile.Volume = globalVolume;
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.PlaybackStopped += OnPlaybackStopped;
                currentTrack = track;
                outputDevice.Play();

                Console.WriteLine($"▶ Riproduzione: {Path.GetFileName(track)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore riproduzione: {ex.Message}");
                // Passa alla traccia successiva in caso di errore
                NextTrack();
            }
        }

        private static void NextTrack()
        {
            if (currentPlaylist == null || currentPlaylist.Count == 0)
                return;

            currentTrackIndex = (currentTrackIndex + 1) % currentPlaylist.Count;

            // Se siamo tornati all'inizio, rimescola la playlist
            if (currentTrackIndex == 0)
            {
                currentPlaylist = currentPlaylist.OrderBy(x => random.Next()).ToList();
                Console.WriteLine("🔀 Playlist rimescolata");
            }

            PlayTrackFromPlaylist();
        }

        private static void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            // Se stiamo stoppando intenzionalmente, ignora
            if (isStopping)
                return;

            // Esegui il cambio traccia sul thread principale per evitare race conditions
            Task.Run(() =>
            {
                // Se c'è una playlist attiva, passa alla traccia successiva
                if (currentPlaylist != null && currentPlaylist.Count > 0)
                {
                    NextTrack();
                    return;
                }

                // Altrimenti loop normale
                if (!loop || audioFile == null || outputDevice == null)
                    return;

                try
                {
                    audioFile.Position = 0;
                    outputDevice.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore nel loop: {ex.Message}");
                }
            });
        }

        /// Regola il volume (0.0 - 1.0)
        public static void SetVolume(float volume)
        {
            globalVolume = Math.Clamp(volume, 0f, 1f);
            if (audioFile != null)
                audioFile.Volume = globalVolume;
        }

        /// <summary>
        /// Abbassa il volume gradualmente (per overlay form)
        /// </summary>
        public static async Task DuckVolumeAsync(float targetVolume = 0.3f, int ms = 500)
        {
            if (audioFile == null) return;

            float start = audioFile.Volume;
            targetVolume = Math.Clamp(targetVolume, 0f, 1f);

            for (int i = 0; i <= 20; i++)
            {
                float progress = i / 20f;
                audioFile.Volume = start + (targetVolume - start) * progress;
                await Task.Delay(ms / 20);
            }
        }

        /// <summary>
        /// Ripristina il volume originale
        /// </summary>
        public static async Task RestoreVolumeAsync(int ms = 500)
        {
            if (audioFile == null) return;

            float start = audioFile.Volume;

            for (int i = 0; i <= 20; i++)
            {
                float progress = i / 20f;
                audioFile.Volume = start + (globalVolume - start) * progress;
                await Task.Delay(ms / 20);
            }
        }

        /// <summary>
        /// Fade-in dolce
        /// </summary>
        public static async Task FadeInAsync(int ms = 1000)
        {
            if (audioFile == null) return;

            audioFile.Volume = 0f;
            for (int i = 0; i <= 20; i++)
            {
                audioFile.Volume = globalVolume * (i / 20f);
                await Task.Delay(ms / 20);
            }
        }

        /// <summary>
        /// Fade-out dolce
        /// </summary>
        public static async Task FadeOutAsync(int ms = 1000)
        {
            if (audioFile == null) return;

            float start = audioFile.Volume;
            for (int i = 0; i < 20; i++)
            {
                audioFile.Volume = start * (1f - (i / 20f));
                await Task.Delay(ms / 20);
            }
            Stop();
        }

        /// <summary>
        /// Ferma e rilascia le risorse
        /// </summary>
        public static void Stop()
        {
            isStopping = true;

            if (outputDevice != null)
            {
                try
                {
                    outputDevice.PlaybackStopped -= OnPlaybackStopped;
                    if (outputDevice.PlaybackState == PlaybackState.Playing)
                        outputDevice.Stop();
                }
                catch { }

                try
                {
                    outputDevice.Dispose();
                }
                catch { }

                outputDevice = null;
            }

            if (audioFile != null)
            {
                try
                {
                    audioFile.Dispose();
                }
                catch { }

                audioFile = null;
            }

            // Resetta lo stato DOPO aver pulito le risorse
            Task.Delay(100).ContinueWith(_ =>
            {
                isStopping = false;
                loop = false;
                currentTrack = null;
                // NON resettare currentPlaylist qui se vogliamo mantenere la playlist attiva
            });
        }

        public static bool IsPlaying => outputDevice?.PlaybackState == PlaybackState.Playing;
    }

    /// <summary>
    /// Gestisce gli effetti sonori (SFX) con sistema di pooling
    /// </summary>
    internal class SoundManager
    {
        private static readonly Dictionary<string, CachedSound> soundCache = new();
        private static readonly List<WaveOutEvent> activeOutputs = new();
        private static float globalVolume = 0.8f;
        private static readonly object lockObj = new();

        /// <summary>
        /// Precache di un suono per performance ottimali
        /// </summary>
        public static void PreloadSound(string file)
        {
            if (soundCache.ContainsKey(file)) return;

            try
            {
                if (!File.Exists(file))
                {
                    Console.WriteLine($"File audio non trovato: {file}");
                    return;
                }
                soundCache[file] = new CachedSound(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore preload suono {file}: {ex.Message}");
            }
        }

        /// <summary>
        /// Riproduce un effetto sonoro
        /// </summary>
        public static void PlaySound(string file, float volume = 1.0f)
        {
            Task.Run(() => PlaySoundInternal(file, volume));
        }

        private static void PlaySoundInternal(string file, float volume)
        {
            try
            {
                if (!soundCache.ContainsKey(file))
                    PreloadSound(file);

                if (!soundCache.ContainsKey(file)) return;

                var sound = soundCache[file];
                var output = new WaveOutEvent();
                var provider = new CachedSoundSampleProvider(sound);

                output.Init(provider);
                output.Volume = Math.Clamp(volume * globalVolume, 0f, 1f);

                lock (lockObj)
                {
                    activeOutputs.Add(output);
                }

                output.PlaybackStopped += (s, e) =>
                {
                    lock (lockObj)
                    {
                        activeOutputs.Remove(output);
                    }
                    output.Dispose();
                };

                output.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore riproduzione suono: {ex.Message}");
            }
        }
        public static void StopAll()
        {
            lock (lockObj)
            {
                foreach (var output in activeOutputs.ToArray())
                {
                    try
                    {
                        output.Stop();
                        output.Dispose();
                    }
                    catch { }
                }
                activeOutputs.Clear();
            }
        }

        public static void ClearCache()
        {
            soundCache.Clear();
        }
    }

    internal class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public CachedSound(string audioFileName)
        {
            using var audioFileReader = new AudioFileReader(audioFileName);
            WaveFormat = audioFileReader.WaveFormat;

            var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
            var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
            int samplesRead;

            while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }

            AudioData = wholeFile.ToArray();
        }
    }

    internal class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        public WaveFormat WaveFormat => cachedSound.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);

            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;

            return (int)samplesToCopy;
        }
    }

    /// Classe helper per gestire l'audio del gioco con playlist
    public static class GameAudio
    {
        private static string GetPath(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        // === PLAYLIST ORGANIZZATE PER CONTESTO ===

        // Menu principale - Playlist casuale
        public static readonly List<string> PLAYLIST_GIOCO = new()
        {
            GetPath("Assets/Sound/Music/sword_5.mp3"),
            GetPath("Assets/Sound/Music/sword_7.mp3"),
            GetPath("Assets/Sound/Music/sword_8.mp3"),
            GetPath("Assets/Sound/Music/shadows-of-souls-ancient-medieval-cinematic-357891.mp3")
        };

        // Login
        public static readonly List<string> PLAYLIST_LOGIN = new()
        {
            GetPath("Assets/Sound/Music/sword_1.mp3")
        };

        // Villaggio
        public static readonly List<string> PLAYLIST_VILLAGE = new()
        {
            GetPath("Assets/Sound/Music/sword_11.mp3")
        };

        // Battaglia PvP
        public static readonly List<string> PLAYLIST_PVP = new()
        {
            GetPath("Assets/Sound/Music/sword_3.mp3"),
            GetPath("Assets/Sound/Music/battle_2.mp3")
        };

        // Costruzioni
        public static readonly List<string> PLAYLIST_BUILD = new()
        {
            GetPath("Assets/Sound/Music/build_1.mp3")
        };

        public static void PlayMenuMusic(string musica)
        {
            if (musica == "Gioco")
                MusicManager.PlayPlaylist(PLAYLIST_GIOCO, shuffle: true);
            if (musica == "Login")
                MusicManager.PlayPlaylist(PLAYLIST_LOGIN, shuffle: true);
            if (musica == "Villaggio")
                MusicManager.PlayPlaylist(PLAYLIST_VILLAGE, shuffle: true);
            if (musica == "PVP")
                MusicManager.PlayPlaylist(PLAYLIST_PVP, shuffle: true);
        }
        public static void StopMusic()
        {
            MusicManager.Stop();
        }
        /// Cleanup completo - SOLO alla chiusura dell'applicazione
        public static void Cleanup()
        {
            MusicManager.Stop();
            SoundManager.StopAll();
            SoundManager.ClearCache();
        }
    }
}