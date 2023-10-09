using UnityEngine;

namespace SkyRoad.Main
{
    public struct GameModel
    {
        private int _score;
        private int _hiScore;
        public float Time;
        public float LastTime;
        public int ObstacleCount;
        public bool NewHiScore;
        public float AdditionalSpeed;
        public float SpeedChangeTimer;
        private readonly string HiScoreKey;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;

                CheckHiScore(_score);
            }
        }

        public int HiScore
        {
            get
            {
                if (_hiScore == 0)
                {
                    _hiScore = GetHiScore();
                }
                
                return _hiScore;
            }
        }

        public void Reset()
        {
            Score = 0;
            Time = 0.0f;
            ObstacleCount = 0;
            NewHiScore = false;
            AdditionalSpeed = 0.0f;
            SpeedChangeTimer = 0.0f;
        }
        
        private void CheckHiScore(int score)
        {
            _hiScore = GetHiScore();

            if (score > _hiScore)
            {
                SetHiScore(_score);
                NewHiScore = true;
            }
        }
        
        private int GetHiScore()
        {
            if (!PlayerPrefs.HasKey(HiScoreKey))
            {
                SetHiScore(_score);
            }

            return PlayerPrefs.GetInt(HiScoreKey);
        }

        private void SetHiScore(int hiScore)
        {
            _hiScore = hiScore;
            PlayerPrefs.SetInt(HiScoreKey, hiScore);
        }
    }
}

