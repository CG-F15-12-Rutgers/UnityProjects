using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree2 : MonoBehaviour
{
	public Transform meeting_point;

	int num = 4;
	public GameObject participant0;
	public GameObject participant1;
	public GameObject participant2;
	public GameObject participant3;
	public GameObject ball;

	public Transform chair0;
	public Transform chair1;
	public Transform chair2;
	public Transform chair3;
	public Transform ball_pos;
	public Transform ball_throw_pos;

	public InteractionObject ball_obj;
	public FullBodyBipedEffector Effector;

	private GameObject[] participants;
	private Transform[] chairs;
	private BehaviorMecanim behaviors;
	private BehaviorAgent behaviorAgent;
	private Vector3 center;

	// Use this for initialization
	void Start ()
	{
		participants = new GameObject[num];
		participants[0] = participant0;
		participants[1] = participant1;
		participants[2] = participant2;
		participants[3] = participant3;

		chairs = new Transform[4];
		chairs[0] = chair0;
		chairs[1] = chair1;
		chairs[2] = chair2;
		chairs[3] = chair3;

		center.x = 13;
		center.y = 1;
		center.z = 23;

		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	// behaviors

	protected Node GoToPoint(int i, float t, Transform point)
	{
		Val<Vector3> position = Val.V(() => point.position);	
		Val<float> distance = Val.V(() => t);
		return new Sequence(participants[i].GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, distance), new LeafWait(100));
	}

	protected Node GoToMeetingPoint(int i, float t, Transform meeting_point, Transform look_at)
	{
		Val<Vector3> position = Val.V(() => meeting_point.position);	
		Val<Vector3> look_pos = Val.V(() => look_at.position);
		Val<float> distance = Val.V(() => t);
		return new SequenceParallel (participants [i].GetComponent<BehaviorMecanim> ().Node_HeadLook (look_pos), 
		                             participants [i].GetComponent<BehaviorMecanim> ().Node_GoToUpToRadius (position, t));		                  
	}

	protected Node Orientation(int i, Transform look_at){
		Val<Vector3> look_pos = Val.V(() => look_at.position);
		return new Sequence(participants[i].GetComponent<BehaviorMecanim>().Node_OrientTowards(look_pos),new LeafWait(100));
	}
	protected Node WaveHand(int i)
	{
		Val<string> gesture = Val.V(() => "WaveHands");
		Val<long> duration = Val.V(() => 1l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(gesture, duration);
	}
	protected Node Punch(int i)
	{
		Val<string> gesture = Val.V(() => "Punch");
		Val<long> duration = Val.V(() => 1l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(gesture, duration);
	}
	protected Node SweepLeg(int i)
	{
		Val<string> gesture = Val.V(() => "SweepLeg");
		Val<long> duration = Val.V(() => 1l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(gesture, duration);
	}
	protected Node FlyKick(int i)
	{
		Val<string> gesture = Val.V(() => "FlyKick");
		Val<long> duration = Val.V(() => 1l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(gesture, duration);
	}
	protected Node Throw(int i)
	{
		Val<string> gesture = Val.V(() => "Throw");
		Val<long> duration = Val.V(() => 1l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(gesture, duration);
	}
	protected Node Clap(int i)
	{
		Val<string> gesture = Val.V(() => "Clap");
		Val<long> duration = Val.V(() => 1000l);
		return participants[i].GetComponent<BehaviorMecanim>().ST_PlayHandGesture(gesture, duration);
	}
	
	protected Node ToChair(int i)
	{
		Val<Vector3> chair_pos = Val.V(() => chairs[i].position);
		Val<Vector3> position = Val.V(() => center);
		return new Sequence(		
			participants[i].GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(chair_pos, 0.4f),
			participants[i].GetComponent<BehaviorMecanim>().Node_OrientTowards(position),new LeafWait(100));
	}

	protected Node SitOnChair(int i)
	{
		return new Sequence(participants[i].GetComponent<BehaviorMecanim>().Node_Sit(), new LeafWait(100));
	}

	protected Node StandUp(int i)
	{
		return new Sequence(participants[i].GetComponent<BehaviorMecanim>().Node_Stand ());
	}

	// ik behaviors
	
	protected Node PickUp(int i)
	{
		Val<FullBodyBipedEffector> VEffector = Val.V(() => Effector);
		Val<InteractionObject> VBall = Val.V(() => ball_obj);
		return new Sequence(participants[i].GetComponent<BehaviorMecanim>().Node_StartInteraction(VEffector, VBall), new LeafWait(100));
	}

	protected RunStatus BallFall(Vector3 direction)
	{
		transform.parent = null;
		ball.GetComponent<Rigidbody>().AddForce(direction);
		ball.GetComponent<Rigidbody>().isKinematic = false;
		ball.GetComponent<Rigidbody>().useGravity = true;

		return RunStatus.Success;
	}

	protected Node BallAfterThrow(int i, Vector3 direction)
	{
		return new LeafInvoke(() => BallFall(direction));
	}

	protected Node ThrowBall(int i)
	{
		Vector3 direction;

		direction =  ball_pos.position - participants [i].transform.position;
		return new SequenceParallel(Throw (i), new Sequence(new LeafWait(600), BallAfterThrow(i, direction)));
	}
	
	protected Node BuildTreeRoot()
	{
		return
			new Sequence (

			new SequenceParallel(

				new Sequence(							
					this.ToChair(0),						
					new Selector2Weighted(this.SitOnChair(0),this.StandUp(0)),
					new LeafWait(100)),

				new Sequence(
					this.ToChair(1),						
					new Selector2Weighted(this.SitOnChair(1),this.StandUp(1)),
					new LeafWait(100)),

				new Sequence(
					this.ToChair(2),						
					new Selector2Weighted(this.SitOnChair(2),this.StandUp(2)),   
					new LeafWait(100)),

				new Sequence(this.GoToMeetingPoint(3, 7.0f, meeting_point, meeting_point),this.WaveHand(3),new LeafWait(100))
			    ),
				 
			new SequenceParallel(
				
				new Sequence(
					this.StandUp(0),	
					this.GoToMeetingPoint(0, 0.5f, meeting_point, participants[3].transform),
					this.Orientation(0, participants[3].transform),
					this.WaveHand(0),
					new LeafWait(100)
				),
				new Sequence(
					this.StandUp(1),						
					this.GoToMeetingPoint(1, 0.5f, meeting_point, participants[3].transform),
					this.Orientation(1, participants[3].transform),
					this.WaveHand(1),
				    new LeafWait(100)
				),
				new Sequence(
					this.StandUp(2),						
					this.GoToMeetingPoint(2, 0.5f, meeting_point, participants[3].transform),
					this.Orientation(2, participants[3].transform),
					this.WaveHand(2),
					new LeafWait(100)					
				)),
	
			new Sequence(
				new LeafWait(10),this.Punch(3),new LeafWait(5000), 
				new SequenceParallel(this.Clap (0),this.Clap (1),this.Clap (2)),
				new LeafWait(10),this.FlyKick(3),new LeafWait(5000), 
				new SequenceParallel(this.Clap (0),this.Clap (1),this.Clap (2)),
				new LeafWait(10),this.SweepLeg(3),new LeafWait(5000), 
				new SequenceParallel(this.Clap (0),this.Clap (1),this.Clap (2))),
			
			new Sequence (
				new Sequence(this.GoToMeetingPoint(2, 3.0f, ball_pos, ball.transform),new LeafWait(1000)),
				new Sequence(this.PickUp(2),new LeafWait(1000)),
				new Sequence(this.ThrowBall(2),new LeafWait(100)),
			    new DecoratorLoop(new LeafWait(100)))
			);
	}
}