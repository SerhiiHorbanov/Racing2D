namespace Lobby
{
	public interface IRacer
	{
		public RaceCar Car { get; set; }
		public void SpawnController(RaceCar car);
		public RacerScore GetScore();
	}
}