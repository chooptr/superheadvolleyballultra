using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	VideoCapture videoCapture;
	private Mat _webcamFrame = new Mat();
	private IOutputArray _webcamFrameGray;
	[Range(10, 60)] public int delay = 24;

	private CascadeClassifier _frontfacesCascadeClassifier;
	private string _frontfacesCascadeClassifierPath = "classifier.xml";
	public float _scaleFactor;
	public int _minNeighbors;
	public int _minSize;
	public int _maxSize;

	private Mat gray = new Mat();
	Rectangle[] faces = null;

	public double r, g, b;

	void Start() {
		videoCapture = new VideoCapture();
		videoCapture.ImageGrabbed += GrabHandler;
		_frontfacesCascadeClassifier = new CascadeClassifier(Path.Combine(Application.dataPath, _frontfacesCascadeClassifierPath));
	}

	private void GrabHandler(object sender, EventArgs e) {
		if (!videoCapture.IsOpened) {
			return;
		}
		videoCapture.Retrieve(_webcamFrame);
		CvInvoke.Flip(_webcamFrame, _webcamFrame, FlipType.Horizontal);

		CvInvoke.CvtColor(_webcamFrame, gray, ColorConversion.Bgr2Gray);

		faces = null;
		try {
			faces = _frontfacesCascadeClassifier.DetectMultiScale(
				gray,
				_scaleFactor,
				_minNeighbors,
				new Size(_minSize, _minSize),
				new Size(_maxSize, _maxSize)
			);

			foreach (Rectangle face in faces) {
				CvInvoke.Rectangle(_webcamFrame, face, new MCvScalar(b, g, r));
				Debug.Log(face.Location);
			}
		}
		catch (Exception ex) {
			Debug.Log(ex.Message);
		}
		
		//CvInvoke.Imshow("webcam.view", _webcamFrame);
		CvInvoke.WaitKey(delay);
	}

	void Update() {
		if (videoCapture.IsOpened) {
			videoCapture.Grab();
		}
	}

	private void OnDestroy() {
		videoCapture.Dispose();
		CvInvoke.DestroyAllWindows();
	}
}