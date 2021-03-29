﻿using System;
using UnityEngine;

//Counts current score and holds best score
public class PlayerProgressionModel : MonoBehaviour
{
    public event Action<int> TurnChanged;
    public event Action<int> CurrentScoreChanged;
    public event Action<int> BestScoreChanged;
    public event Action<int> BestRuneChanged;
    public event Action<int> LevelNumberChanged;

    public int CurrentScore
    {
        get { return currentScore; }
        private set
        {
            currentScore = value;
            CurrentScoreChanged(value);
        }
    }

    public int LevelNumber
    {
        get { return levelNumber; }
        set
        {
            levelNumber = value;
            LevelNumberChanged(value);
        }
    }

    public int TurnNumber
    {
        get { return turnNumber; }
        set
        {
            turnNumber = value;
            TurnChanged(value);
        }
    }

    public int BestScore
    {
        get { return bestScore; }
        set
        {
            bestScore = value;
            BestScoreChanged(value);
        }
    }

    public int BestLevel { get; set; }
    public int BestRune { get; set; }
    public int Stage { get; set; }
    public int TotalMerged { get; set; }

    GridModel gridModel;
    int currentScore;
    int levelNumber;
    int turnNumber;
    int bestScore;

    void Awake()
    {
        gridModel = GetComponent<GridModel>();
        gridModel.GridChanged += OnGridChanged;
        gridModel.CellsMerged += OnCellsMerged;
        LevelNumberChanged += UpdateBestLevel;
    }

    void OnDestroy()
    {
        gridModel.GridChanged -= OnGridChanged;
        gridModel.CellsMerged -= OnCellsMerged;
        LevelNumberChanged -= UpdateBestLevel;
    }

    public void Initialize(int currentScore, int levelNumber, int turnNumber, int bestScore, int bestLevel, int bestRune, int stage, int totalMerged)
    {
        CurrentScore = currentScore;
        LevelNumber = levelNumber;
        BestScore = bestScore;
        BestLevel = bestLevel;
        BestRune = bestRune;
        Stage = stage;
        TotalMerged = totalMerged;
        TurnNumber = turnNumber;
    }

    void OnGridChanged(Vector2Int[] area, int level)
    {
        if (level == 0)
        {
            int score = 0;
            if (area.Length > 25)
            {
                score = 100;
            }
            else if (area.Length > 16)
            {
                score = 50;
            }
            else if (area.Length > 9)
            {
                score = 25;
            }
            else if (area.Length > 4)
            {
                score = 10;
            }
            else if (area.Length > 1)
            {
                score = 5;
            }
            CurrentScore += score;
        }

        //TODO LOW: Cache
        if (level > BestRune && level <= GetComponent<Resources>().TilesList.Length)
        {
            BestRune = level;
            BestRuneChanged?.Invoke(BestRune);
        }
    }

    void OnCellsMerged(int count)
    {
        TotalMerged += count;
    }

    public void UpdateBestScore()
    {
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
    }

    void UpdateBestLevel(int level)
    {
        if (levelNumber > BestLevel)
        {
            BestLevel = levelNumber;
        }
    }
}