namespace ConnectedFoods.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public int Level { get; set; }
        public int TotalGameScore { get; set; }

        private void Start()
        {
            Level = 1;
        }
    }
}
