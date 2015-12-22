using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;
public class BehaviorTree2 : MonoBehaviour {

   	public GameObject soldier;
	public GameObject roma;
	public GameObject door;

    public Transform fighting_point;
	public Transform door_point;
	public Transform Button;
	public Transform jail_point;

	private BehaviorMecanim behaviors;
	private BehaviorAgent behaviorAgent;

    // Use this for initialization
	void Start () {

        GameObject[] person;
        person = GameObject.FindGameObjectsWithTag("Player");

		GameObject[] points;
		person = GameObject.FindGameObjectsWithTag("meeting_point");
		Transform[] meeting_point = new Transform[points.Length];
		int i = 0;
		foreach(GameObject point in points){
			meeting_point[i] = point.GetComponent<Transform>();
			i++;
			}

		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//----------------------------- single behaviors ---------------------------------

	protected Node GoToPoint(GameObject person, Transform point)
	{
		Val<Vector3> position = Val.V(() => point.position);	
		return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(2000));
	}

	protected Node GoToPointAround(GameObject person, Transform point, float r)
	{
		Val<Vector3> position = Val.V(() => point.position);	
		Val<float> distance = Val.V(() => r);
		return new Sequence(person.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position, distance), new LeafWait(2000));
	}

	protected Node Orientation(GameObject person, Transform target){
		Val<Vector3> position = Val.V(() => target.position);
		return new Sequence(person.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(500));
	}

	protected Node LookAt(GameObject person, Transform target)
	{
		Val<Vector3> position = Val.V(() => target.position);	
		return new Sequence(person.GetComponent<BehaviorMecanim> ().Node_HeadLook (position), new LeafWait(500));		                  
	}
		
	// gesture: "WaveHands"  "Clap" 1000l  "FlyKick"  "Punch"  "Dying"  "WaveSword"|   duration: 1l  1000l   
	protected Node Gesture(GameObject person, string gesture, long duration) //duration 1l 1000l 
	{
		Val<string> ges = Val.V(() => gesture);
		Val<long> dur = Val.V(() => duration);
		return new Sequence(person.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture(ges, dur), new LeafWait(500));
	}

	// gesture: "Pointing"  |   duration: 1l
	protected Node HandGesture(GameObject person, string gesture, long duration) //duration 1l 1000l 
	{
		Val<string> ges = Val.V(() => gesture);
		Val<long> dur = Val.V(() => duration);
		return new Sequence(person.GetComponent<BehaviorMecanim>().ST_PlayHandGesture(ges, dur), new LeafWait(500));
	}

	protected Node Speak(GameObject person, string words, long duration)
	{
		return new Sequence (person.GetComponent<BehaviorMecanim>().Node_SpeechBubble(words, duration), new LeafWait (1000));
	}

	// door
	public virtual RunStatus DoorOpenState(GameObject door)
	{
		door.GetComponent<Door>().Open();
		return RunStatus.Success;
	}

	public virtual RunStatus DoorCloseState(GameObject door)
	{
		door.GetComponent<Door>().Close();
		return RunStatus.Success;
	}
	public Node JustOpenDoor(GameObject door)
	{
		return new LeafInvoke(() => this.DoorOpenState(door));
	}
	public Node JustCloseDoor(GameObject door)
	{
		return new LeafInvoke(() => this.DoorCloseState(door));
	}

	//----------------------------- events ---------------------------------

	protected Node OpenDoor(GameObject person, GameObject door)
	{
		return new Sequence(
			this.GoToPointAround(person, door_point, 0.5f),
			this.Orientation(person, Button),
			this.HandGesture(person, "Pointing", 300L),
			this.JustOpenDoor (door),
			this.GoToPointAround(person, meeting_point[6], 2.0f)
		);
	}

	protected Node CloseDoor(GameObject person, GameObject door)
	{
		return new Sequence(
			this.GoToPointAround(person, door_point, 0.5f),
			this.Orientation(person, Button),
			this.HandGesture(person, "Pointing", 300L),
			this.JustCloseDoor (door),
			this.GoToPointAround(person, meeting_point6, 2.0f)
		);
	}

	protected Node Wander(GameObject person, Transform point1, Transform point2)
	{
		return new Sequence (
			this.GoToPointAround (person, point1, 7f),
			new LeafWait (1000),
			this.GoToPointAround (person, point2, 7f)
		);
	}

	protected Node KillingArc(GameObject roma, GameObject person)
	{
		return new Sequence (
			this.GoToPointAround(roma, person.transform, 3.5f),
			this.Orientation(roma, person.transform),
			this.Gesture(roma, "WaveSword", 1L),
			this.Gesture(person, "Dying", 1L)
		);
	}

	protected Node MeetingArc(GameObject person1, GameObject person2, Transform meeting_point)
	{
		return new Sequence (

			new SequenceParallel(
				this.GoToPointAround(person1, meeting_point, 1.0f),
				this.GoToPointAround(person2, meeting_point, 1.0f)
			),

			this.Orientation(person1, person2.transform),
			this.Orientation(person2, person1.transform),
			this.Gesture(person1, "WaveHands", 1L),
			this.Gesture(person2, "WaveHands", 1L),

			new SelectorShuffle(
				Speak(person1, "Hello!", 1000L),
				Speak(person1, "Hi!" , 1000L),
				Speak(person1, "How is it going?", 1000L)			
			),
			new SelectorShuffle(
				Speak(person2, "Hello!", 1000L),
				Speak(person2, "Hi!" , 1000L),
				Speak(person2, "How is it going?", 1000L)			
			),
			new SelectorShuffle(
				Speak(person1, "Do you like this place?", 1000L),
				Speak(person1, "Have fun!" , 1000L),
				Speak(person1, "Great!", 1000L)			
			),
			new SelectorShuffle(
				Speak(person1, "Do you like this place?", 1000L),
				Speak(person1, "Have fun!" , 1000L),
				Speak(person1, "Great!", 1000L)
			)
		);
	}

	protected Node FightingArc(GameObject police, GameObject roma, Transform fighting_point)
	{
		return new Sequence (
			new SequenceParallel(
				this.GoToPointAround(police, fighting_point, 1.5f),
				this.GoToPointAround(roma, fighting_point, 1.5f)),
			this.Orientation(police, roma.transform),
			this.Orientation(roma, police.transform),
			
			Speak(police, "Go back! This is not where you belong!", 1000L),
			Speak(roma, "Nah...", 1000L),

			new Sequence (
				this.Gesture(roma, "WaveSword", 1L),
				this.Gesture(roma, "WaveSword", 1L),
				new SelectorShuffle(
					this.Gesture(police, "FlyKick", 1L),
					this.Gesture(police, "Punch", 1L))
			),

			new SelectorShuffle(
				new Sequence(
					this.Gesture(roma, "WaveSword", 1L),
					this.Gesture(soldier, "Dying", 1L), 
					Speak(roma, "Win!", 500L)),
				new Sequence(
					Speak(soldier, "You lose! Now go back to your jail!", 500L),
					Speak(roma, "Fine!", 500L),
					this.GoToPointAround (roma, jail_point, 1f),
					this.GoToPointAround (soldier, Button, 3f), 		
					CloseDoor(soldier, door))
			)
		);
	}


	// ------------------- behavior tree ---------------------
    protected Node BuildTreeRoot()
	{
		return new Sequence (

			new SequenceParallel (

				new DecoratorLoop(this.MeetingArc(person1, person2, meeting_point1)),
				new DecoratorLoop(this.MeetingArc(person3, person4, meeting_point2)),
				new DecoratorLoop(this.MeetingArc(person5, person6, meeting_point4)),
				new DecoratorLoop(this.MeetingArc(person7, person8, meeting_point6)),

				new DecoratorLoop(Wander(person9, meeting_point2, meeting_point3)),
				new DecoratorLoop(Wander(person10, meeting_point1, meeting_point2)),
				//new DecoratorLoop(Wander(person11, meeting_point4, meeting_point5)),  //stop after kill
				//new DecoratorLoop(Wander(person12, meeting_point5, meeting_point6)),

		   		new Sequence (
					OpenDoor (person12, door),
					this.KillingArc(roma, person11),
					this.KillingArc(roma, person12),
					this.FightingArc(soldier, roma, fighting_point)
				),

				DecoratorLoop(new LeafWait(100))
			)
		);   //not fighting
	}
}
