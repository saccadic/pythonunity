using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Python.Runtime;
using Cysharp.Threading.Tasks;

public class py_hello_world : MonoBehaviour
{
    public py_runtime python;
    public bool bStart;
    private IntPtr thread_state;

    private async UniTask PythonProcess()
    {
        await UniTask.SwitchToThreadPool();

        PythonEngine.Initialize(mode: ShutdownMode.Reload);
        thread_state = PythonEngine.BeginAllowThreads();

        try
        {
            using (Py.GIL())
            {
                Debug.Log("Start Python Process");

                dynamic sysModule = Py.Import("sys");
                sysModule.path.append(python.scripts);

                Debug.Log(sysModule.version);

                dynamic os = Py.Import("os");
                Debug.Log(os.getcwd());

                Debug.Log("Stop Python Process");
            }
        }
        catch (PythonException e)
        {
            Debug.Log(e.ToString());
        }

        PythonEngine.EndAllowThreads(thread_state);
        PythonEngine.Shutdown(mode: ShutdownMode.Reload);

        await UniTask.SwitchToMainThread();
    }

    public async void run()
    {
        if (!python.IsEnabled() && bStart)
            return;

        await PythonProcess();

        bStart = true;
    }

    public void stop()
    {
        if (!python.IsEnabled() && !bStart)
            return;

        bStart = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            run();
        }
    }
}
