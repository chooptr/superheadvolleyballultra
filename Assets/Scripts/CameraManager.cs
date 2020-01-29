using System;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraManager : MonoBehaviour {
	public PlayerController pL, pR;

	[Range(10, 60)] public int delay = 24;
	public float scaleFactor;
	public int minNeighbors;
	public int minSize;
	public int maxSize;
	public double r, g, b;

	private CascadeClassifier _frontfacesCascadeClassifier;
	private string _frontfacesCascadeClassifierPath = "classifier.xml";
	private Mat gray = new Mat();
	private Mat _webcamFrame = new Mat();
	private VideoCapture videoCapture;
	private Rectangle[] faces = null;
	private Rectangle leftMost, rightMost;

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
				scaleFactor,
				minNeighbors,
				new Size(minSize, minSize),
				new Size(maxSize, maxSize)
			);

			//var leftFaces = faces.OrderBy(x => x.Location.X).Select();
			var ordered = faces.OrderBy(x => x.Location.X);

			leftMost = ordered.First();
			rightMost = ordered.Last();
			Debug.Log(leftMost.Location);
			CvInvoke.Rectangle(_webcamFrame, leftMost, new MCvScalar(b, g, r));
			CvInvoke.Rectangle(_webcamFrame, rightMost, new MCvScalar(r, g, b));

		}
		catch (Exception ex) {
			Debug.Log(ex.Message);
		}
		
		CvInvoke.Imshow("webcam.view", _webcamFrame);
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