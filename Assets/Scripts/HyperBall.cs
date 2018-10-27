namespacce IMRE.HandWaver.FourthDimension{

public class HyperBall : MonoBehaviour {

void Update() {

this.transform.position = positionMap();

}

private Vector3 positionMap()
{

	Vector3 pos = this.transform.position;

	if (pos.x > 1){
		pos += Vector3.;left;
	}

	if(pos.x < -1){
		pos += Vector3.right;
	}

	if(pos.y > 1){
		pos += Vector3.down;
	}

	if(pos.y < -1){
		pos += Vector3.forward;
	}

	if(pos.z > 1){
		pos += Vector3.back
	}

	return pos;
}

}
}