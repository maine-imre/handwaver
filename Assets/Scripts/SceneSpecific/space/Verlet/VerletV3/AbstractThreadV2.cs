using System.Collections;
using System.Threading;
using UnityEngine;

public class AbstractThreadV2 {				//This is an abstract class defining various components of a thread
	private bool m_IsDone = false;			//True if the thread is done
	private object m_Handle = new object();	//This is the actual thread
	private Thread m_Thread = null;
	private string m_Name = null;			//Name of the thread

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

	public virtual void Start () {		//Start the thread
		m_Thread = new Thread(Run);
		m_Thread.Start();
	}
	public virtual void Abort () {		//Abort the thread
		m_Thread.Abort();
	}
	protected virtual void ThreadedFunction() {}	//This is the function that is run in the thread

	protected virtual float OnFinished() {return new float();}	

	public virtual bool Update() {		//On update if the frame is done
		if(IsDone) {
			OnFinished();
			return true;
		}
		return false;
	}

	public IEnumerator WaitFor() {		//This may be what is causing problems
		while(!Update()){
			Thread.Sleep(20);
		}
		if(!Update()){
			yield return null;
		}
	}

	//public IEnumerator WaitFor() {		//This may be what is causing problems
	//	while(!Update()) {
	//		yield return null;
	//	}
	//}

	public void Run() {					//Runs the thread
		ThreadedFunction();
		IsDone = true;
	}
}
