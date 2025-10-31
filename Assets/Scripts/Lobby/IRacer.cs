namespace Lobby
{
	public interface IRacer
	{
		public RaceCar Car { get; set; }
		
		public void ConnectCarControllerTo(RaceCar car);

		public void DisconnectCarControllerFromCar();
		
		public RacerScore GetScore();
	}
}