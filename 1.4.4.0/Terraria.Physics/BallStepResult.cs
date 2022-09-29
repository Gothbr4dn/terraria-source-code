namespace Terraria.Physics
{
	public struct BallStepResult
	{
		public readonly BallState State;

		private BallStepResult(BallState state)
		{
			State = state;
		}

		public static BallStepResult OutOfBounds()
		{
			return new BallStepResult(BallState.OutOfBounds);
		}

		public static BallStepResult Moving()
		{
			return new BallStepResult(BallState.Moving);
		}

		public static BallStepResult Resting()
		{
			return new BallStepResult(BallState.Resting);
		}
	}
}
