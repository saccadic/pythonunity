using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using Python.Runtime;

public class py_runtime : MonoBehaviour
{
    public string pythonHome;
    public string scripts;
    public bool bInitPyRuntime;
    public IntPtr thread_state;

    // Start is called before the first frame update
    void Start()
    {
        bInitPyRuntime = false;

        pythonHome = $"{Directory.GetCurrentDirectory()}/Runtime/python";
        scripts = $"{Directory.GetCurrentDirectory()}/Runtime/scripts";
        Environment.SetEnvironmentVariable("PATH", $"{pythonHome};{pythonHome}/{scripts}", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("DYLD_LIBRARY_PATH", $"{pythonHome}/Lib;{pythonHome}/Lib/site-packages/torch/lib", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", $"{pythonHome}/python38.dll", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("TCL_LIBRARY", $"{pythonHome}/tcl/tcl8.6", EnvironmentVariableTarget.Process);
        PythonEngine.PythonHome = $"{pythonHome}";
        PythonEngine.PythonPath = $"{pythonHome}/DLLs;{pythonHome}/Lib;{pythonHome}/Lib/site-packages;{pythonHome}/Lib/site-packages/torch/lib/";

        bInitPyRuntime = true;
    }

    // Update is called once per frame
    void OnDisable()
    {
        if (PythonEngine.IsInitialized)
        {
            PythonEngine.Shutdown(mode: ShutdownMode.Reload);

            Debug.Log("Close:PythonEngine");
        }
    }

    public bool IsEnabled()
    {
        return bInitPyRuntime;
    }
}
