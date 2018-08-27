using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AbstractThread {
	private bool m_IsDone = false;
	private object m_Handle = new object();
	private Thread m_Thread = null;
	private string m_Name = null;

	public string ThreadName {
		get {
			string tmp;
			lock (m_Handle) {
				tmp = m_Name;
			}
			return tmp;
		}
		set {
			lock (m_Handle) {
				m_Name = value;
			}
		}
	}

	public bool IsDone {
		get {
			bool tmp;
			lock (m_Handle) {
				tmp = m_IsDone;
			}
			return tmp;
		}
		set {
			lock (m_Handle) {
				m_IsDone = value;
			}
		}
	}

	public virtual void Start () {
		m_Thread = new Thread(Run);
		m_Thread.Start();
	}
	public virtual void Abort () {
		m_Thread.Abort();
	}
	protected virtual void ThreadedFunction() {}

	protected virtual Vector3d OnFinished() {return new Vector3d();}

	public virtual bool Update() {
		if(IsDone) {
			OnFinished();
			return true;
		}
		return false;
	}

	public IEnumerator WaitFor() {
		while(!Update()) {
			yield return null;
		}
	}

	public void Run() {
		ThreadedFunction();
		IsDone = true;
	}
}
