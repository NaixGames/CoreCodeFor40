using CoreCode.Scripts;
using Godot;

namespace CoreCode.Scripts{
	public partial class SingleNodeExample : SingleNode<SingleNodeExample>
	{
		public void PrintTest(){
			GD.Print("THIS SHOULD WORK!");
		}
	}
}