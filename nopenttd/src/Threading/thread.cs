/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file thread.h Base of all threads. */

using System;
using System.Diagnostics;
using System.Threading;
using Nopenttd.src.Core.Exceptions;

namespace Nopenttd.Threading
{
    /** Definition of all thread entry functions. */
    delegate void OTTDThreadFunc();


    /** Signal used for signalling we knowingly want to end the thread. */
    class OTTDThreadExitSignal { };



    /**
* Win32 thread version for ThreadObject.
*/
    public class ThreadObject : IDisposable
    {

        private Thread thread;  /// System thread identifier.
        private bool self_destruct;  /// Free ourselves when done?
        private uint id;             /// Thread identifier.
        private OTTDThreadFunc proc; /// External thread procedure.
        //??? void* param;         /// Parameter for the external thread procedure.

        /**
    * Create a win32 thread and start it, calling proc(param).
*/
        public ThreadObject(ThreadStart proc, /*void* param,*/ bool self_destruct, string name) 
        {
            thread = null;
            id = 0;
            //this.param = ParameterizedThreadStart;
            this.self_destruct = self_destruct;
            thread = new Thread(proc);
            thread.Name = name;
            thread.Start();
        }


        /**
         * Create a thread; proc will be called as first function inside the thread,
         *  with optional params.
         * @param proc The procedure to call inside the thread.
         * @param param The params to give with 'proc'.
         * @param thread Place to store a pointer to the thread in. May be NULL.
         * @param name A name for the thread. May be NULL.
         * @return True if the thread was started correctly.
         */
        public static bool New(OTTDThreadFunc proc, void *param, ThreadObject **thread = null, const char *name = null);


        /* virtual */
        ~ThreadObject()
        {
            ReleaseUnmanagedResources();
        }

        /* virtual */
        public bool Exit()
        {
            Debug.Assert(Thread.CurrentThread == this.thread);
            /* For now we terminate by throwing an error, gives much cleaner cleanup */
            //thread.Abort();
            throw OTTDThreadExitSignal();
        }

        /* virtual */
        public void Join()
        {
            /* You cannot join yourself */
            Debug.Assert(Thread.CurrentThread != this.thread);
            this.thread.Join(); //infinite
        }
        
        /**
         * A new thread is created, and this function is called. Call the custom
         *  function of the creator of the thread.
         */
        private void ThreadProc()
        {
            try
            {
                this->proc(this->param);
            }
            catch (OTTDThreadExitSignal)
            {
            }
            catch
            {
                throw new NotReachedException();
            }

            if (self_destruct) delete this;
        }

        private void ReleaseUnmanagedResources()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
                //CloseHandle(thread);
                thread = null;
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
    };

        /* static */
        bool ThreadObject::New(OTTDThreadFunc proc, void* param, ThreadObject** thread, const char* name)
{
    ThreadObject* to = new ThreadObject_Win32(proc, param, thread == null, name);
    if (thread != null) *thread = to;
    return true;
}

};

/**
 * Cross-platform Mutex
 */
public abstract class ThreadMutex {

	/**
	 * Create a new mutex.
	 */
	public static ThreadMutex New();
        
	/**
	 * Begin the critical section
	 * @param allow_recursive Whether recursive locking is intentional.
	 *                        If false, NOT_REACHED() will be called when the mutex is already locked
	 *                        by the current thread.
	 */
	public abstract void BeginCritical(bool allow_recursive = false);

	/**
	 * End of the critical section
	 * @param allow_recursive Whether recursive unlocking is intentional.
	 *                        If false, NOT_REACHED() will be called when the mutex was locked more
	 *                        than once by the current thread.
	 */
	public abstract void EndCritical(bool allow_recursive = false);

        /**
         * Wait for a signal to be send.
         * @pre You must be in the critical section.
         * @note While waiting the critical section is left.
         * @post You will be in the critical section.
         */
        public abstract void WaitForSignal();

        /**
         * Send a signal and wake the 'thread' that was waiting for it.
         */
        public abstract void SendSignal();
};

/**
 * Simple mutex locker to keep a mutex locked until the locker goes out of scope.
 */
public class ThreadMutexLocker {

	/**
	 * Lock the mutex and keep it locked for the life time of this object.
	 * @param mutex Mutex to be locked.
	 */
	public ThreadMutexLocker(ThreadMutex mutex)
	{
	    this.mutex = mutex;
            mutex.BeginCritical();
        }

	/**
	 * Unlock the mutex.
	 */
	~ThreadMutexLocker() { mutex->EndCritical(); }

    private ThreadMutexLocker(const ThreadMutexLocker &) { NOT_REACHED(); }
    private ThreadMutexLocker &operator=(const ThreadMutexLocker &) { NOT_REACHED(); return *this; }
    private ThreadMutex mutex;
};

/**
 * Get number of processor cores in the system, including HyperThreading or similar.
 * @return Total number of processor cores.
 */
uint GetCPUCoreCount();





/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file thread_win32.cpp Win32 thread implementation of Threads. */


/**
 * Win32 thread version of ThreadMutex.
 */
class ThreadMutex_Win32 : public ThreadMutex {
private:
	CRITICAL_SECTION critical_section; ///< The critical section we would enter.
HANDLE event;                      ///< Event for signalling.
	uint recursive_count;     ///< Recursive lock count.

public:

    ThreadMutex_Win32() : recursive_count(0)
{
    InitializeCriticalSection(&this->critical_section);
    this->event = CreateEvent(null, FALSE, FALSE, null);
}

/* virtual */
~ThreadMutex_Win32()
{
    DeleteCriticalSection(&this->critical_section);
    CloseHandle(this->event);
}

/* virtual */
void BeginCritical(bool allow_recursive = false)
{
    /* windows mutex is recursive by itself */
    EnterCriticalSection(&this->critical_section);
    this->recursive_count++;
    if (!allow_recursive && this->recursive_count != 1) NOT_REACHED();
}

/* virtual */
void EndCritical(bool allow_recursive = false)
{
    if (!allow_recursive && this->recursive_count != 1) NOT_REACHED();
    this->recursive_count--;
    LeaveCriticalSection(&this->critical_section);
}

/* virtual */
void WaitForSignal()
{
    assert(this->recursive_count == 1); // Do we need to call Begin/EndCritical multiple times otherwise?
    this->EndCritical();
    WaitForSingleObject(this->event, INFINITE);
    this->BeginCritical();
}

/* virtual */
void SendSignal()
{
    SetEvent(this->event);
}
};

/* static */ ThreadMutex* ThreadMutex::New()
{
    return new ThreadMutex_Win32();
}

