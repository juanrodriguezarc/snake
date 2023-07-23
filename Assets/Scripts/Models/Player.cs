using System;

namespace Snake.Models
{
    [Serializable]
    public class Player
    {
        public int id;
        public string name;
        public int score;

        public Player() { }

        public Player(int id, string name, int score)
        {
            this.id = id;
            this.name = name;
            this.score = score;
        }
    }


    
}