using UnityEngine;
using UnityEngine.Profiling;

public class ProfilerHelper : MonoBehaviour
{
    [Header("Profiler Settings")]
    [Tooltip("Maximum memory the profiler can use (in MB). Default is 128 MB.")]
    public int maxProfilerMemoryMB = 512;

    [Header("Profiler Modules")]
    [Tooltip("Disable unused modules to reduce data volume.")]
    public bool disableAudio = true;
    public bool disablePhysics = true;
    public bool disableNetwork = true;
    public bool disableVideo = true;

    void Awake()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        // Increase buffer size
        Profiler.maxUsedMemory = maxProfilerMemoryMB * 1024 * 1024;
        Debug.Log($"[ProfilerHelper] Set Profiler.maxUsedMemory = {maxProfilerMemoryMB} MB");

        // Disable selected modules to reduce load
#if UNITY_2020_2_OR_NEWER
        //if (disableAudio) UnityEngine.Profiling.Profiler.enableBinaryLogRecording = false;
#endif
      //  if (disableAudio) UnityEngine.Profiling.Profiler.logFile = null; // prevents audio logs
      //  if (disablePhysics) ProfilerDriver.SetModuleRecording(ProfilerArea.Physics, false);
      //  if (disableNetwork) ProfilerDriver.SetModuleRecording(ProfilerArea.NetworkMessages, false);
      //  if (disableVideo) ProfilerDriver.SetModuleRecording(ProfilerArea.Video, false);

        Debug.Log("[ProfilerHelper] Applied profiler optimizations.");
#endif
    }
}
