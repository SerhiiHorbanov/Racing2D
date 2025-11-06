using Data;

namespace Lobby
{
	public interface IRacer
	{
		public RaceCar Car { get; set; }
		public RacerConfiguration Configuration { get; set; }
		
		public void EnableCarController(RaceCar car);

		public void DisableCarController();

		public void ConnectRacerCursorControllerTo(RacerCursor cursor);
		
		public void DisconnectRacerCursorControllerFromCursor();
		
		public RacerScore GetScore();
	}
}